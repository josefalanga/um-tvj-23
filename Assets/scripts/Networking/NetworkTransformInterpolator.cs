using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransformInterpolator : NetworkTransform
{
    float lerpFactor = 5;

    Vector3 lastNetworkPosition, lastFramePosition;
    Quaternion lastNetworkRotation, lastFrameRotation;

    float m_lastSyncTime = -1;
    float _lastSyncTime
    {
        set
        {
            if (m_lastSyncTime != value)
            {
                m_lastSyncTime = value;
                hasSyncronized = true;
            }
        }

        get { return m_lastSyncTime; }
    }

    bool hasSyncronized = false;

    //usamos late update porque el update de esta clase ya está ocupado
    //en la clase base de la que heredamos, entonces si escribimos en el
    //update, tenemos resultados impredecibles
    void LateUpdate()
    {
        //solo para objetos que no sean el player local
        if (isLocalPlayer) return;

        //solo para cuando está seleccionado el modo SyncTransform. Asi evitamos problemas con rigidbodies que tienen su propia interpolacion
        if (transformSyncMode != TransformSyncMode.SyncTransform) return;

        _lastSyncTime = this.lastSyncTime;

        if (hasSyncronized)
        {
            //last sync fue este frame, actualizamos nuestro objetivo
            lastNetworkPosition = this.transform.position;
            lastNetworkRotation = this.transform.rotation;
            hasSyncronized = false;
        }

        lastFramePosition = Vector3.Lerp(lastFramePosition, lastNetworkPosition, Time.deltaTime * lerpFactor);
        lastFrameRotation = Quaternion.Lerp(lastFrameRotation, lastNetworkRotation, Time.deltaTime * lerpFactor);

        this.transform.position = lastFramePosition;
        this.transform.rotation = lastFrameRotation;
    }
}
