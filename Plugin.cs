using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace SpinEverything
{
    [BepInPlugin("com.yourname.baldiplus.spineverything", "Spin Everything", "1.0.0")]
    [DefaultExecutionOrder(32000)] // run after the game's own LateUpdates (billboards etc.)
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<float> spinSpeed;
        private SpriteRenderer[] sprites = new SpriteRenderer[0];
        private float nextRefresh;

        private void Awake()
        {
            spinSpeed = Config.Bind("General", "SpinSpeed", 1080f,
                "Degrees per second (1080 = 3 full spins/sec). Negative spins the other way.");
            Logger.LogInfo("Spin Everything loaded!");
        }

        private void LateUpdate()
        {
            // Re-scan for new sprites once a second (cheap enough, catches newly spawned stuff)
            if (Time.unscaledTime >= nextRefresh)
            {
                sprites = FindObjectsOfType<SpriteRenderer>();
                nextRefresh = Time.unscaledTime + 1f;
            }

            float angle = spinSpeed.Value * Time.deltaTime;
            foreach (SpriteRenderer sr in sprites)
            {
                if (sr != null)
                    sr.transform.Rotate(0f, 0f, angle, Space.Self);
            }
        }
    }
}
