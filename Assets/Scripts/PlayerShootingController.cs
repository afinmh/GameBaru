using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] BulletTimeController bulletTimeController;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletSpawnTransform;
    [SerializeField] Scope scope;
    [SerializeField] private float shootingForce;
    [SerializeField] private float minDistanceToPlayAnimation;
    [SerializeField] private float reloadDuration = 1.5f;
    [SerializeField] private TextMeshProUGUI ammoText;
    private bool isScopeEnabled = false;
    private float scrollInput = 0f;
    private bool isShooting = false;
    private bool wasScopeOn;
    
    private bool isReloading = false;
    private int currentAmmo = 1;
    private const int maxAmmo = 1;

    private void Update()
    {
        GetInput();
        HandleScope();
        HandleShooting();
        UpdateAmmoUI();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetMouseButtonDown(1))
            isScopeEnabled = !isScopeEnabled;

        bool tryToShoot = Input.GetMouseButtonDown(0);
        if (tryToShoot && isScopeEnabled)
        {
            if (isReloading)
            {
                isShooting = false;
            }
            else if (currentAmmo > 0)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
                AudioManager.Instance.PlayBulletEmpty();
            }
        }
        else
        {
            isShooting = false;
        }
        scrollInput = Input.mouseScrollDelta.y;
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
    }

    private void HandleShooting()
    {
        if (isShooting)
        {
            currentAmmo--;
            AudioManager.Instance.PlayShootingSound();
            AudioManager.Instance.PlayTrailSound();
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit))
        {
            EnemyController controller = hit.collider.GetComponentInParent<EnemyController>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;

            if (controller)
            {
                controller.StopPatrol();
                if (direction.magnitude >= minDistanceToPlayAnimation)
                {
                    controller.StopAnimation();
                    Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    bulletInstance.Launch(shootingForce, hit.collider.transform, hit.point);
                    bulletTimeController.StartSequence(bulletInstance, hit.point);
                }
                else
                {
                    controller.OnEnemyShot(direction, hit.collider.GetComponent<Rigidbody>());
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.PlayReloadSound();

        if (isScopeEnabled)
        {
            isScopeEnabled = false;
            scope.ResetScopeFOV();
        }

        scope.ReloadOn();
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadDuration);
        currentAmmo = maxAmmo;
        isReloading = false;
        scope.ReloadOff();
        Debug.Log("Reloaded!");
    }

    private void HandleScope()
    {
        scope.ChangeScopeFOV(-scrollInput);
        if (!wasScopeOn)
            scope.ResetScopeFOV();
        scope.SetScopeFlag(isScopeEnabled);
        wasScopeOn = isScopeEnabled;
    }
}
