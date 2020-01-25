using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension {
    public static void Spawn(this GameObject gameObject, Vector3 position) {
        gameObject.transform.position = position;
    }

    public static void Spawn(this GameObject gameObject, Transform spawnPoint) {
        gameObject.transform.position = spawnPoint.position;
        gameObject.transform.rotation = spawnPoint.rotation;
    }
}
