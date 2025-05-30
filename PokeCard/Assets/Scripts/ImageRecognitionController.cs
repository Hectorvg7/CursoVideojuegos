using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionController : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager trackedImageManager;

    [System.Serializable]
    public struct NamedPrefab
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField] private NamedPrefab[] namedPrefabs;

    // Diccionario para acceso rápido por nombre
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // Instanciamos y desactivamos todos los prefabs al inicio
        foreach (var namedPrefab in namedPrefabs)
        {
            GameObject instance = Instantiate(namedPrefab.prefab, Vector3.zero, Quaternion.identity);
            instance.name = namedPrefab.name;
            instance.SetActive(false);
            prefabDictionary[namedPrefab.name] = instance;
        }
    }

    void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var trackedImage in args.removed)
            UpdateImage(trackedImage.Value);
        foreach (var trackedImage in args.added)
            UpdateImage(trackedImage);
        foreach (var trackedImage in args.updated)
            UpdateImage(trackedImage);
    }

    void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveAllListeners();
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (prefabDictionary.TryGetValue(imageName, out GameObject obj))
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                // Actualizar posición y rotación solo si está en tracking
                obj.transform.position = trackedImage.transform.position;
                obj.transform.rotation = trackedImage.transform.rotation;
                obj.SetActive(true);
            }
            else
            {
                // Desactivar objeto si no está en tracking
                obj.SetActive(false);
            }
        }
    }
}
