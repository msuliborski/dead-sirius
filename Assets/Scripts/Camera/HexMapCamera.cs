using UnityEngine;
//using UnityEngine.PostProcessing;
using System.Collections;
using System.Collections.Generic;



public class HexMapCamera : MonoBehaviour
{
    
    [SerializeField]
    private float _scrollBorderThickness = 0.005f;  // percentage of screen height

    [SerializeField]
    private float _stickMinZoom = -250f;
    [SerializeField]
    private float _stickMaxZoom = 0f;
    [SerializeField]
    private float _swivelMinZoom = 90f;
    [SerializeField]
    private float _swivelMaxZoom = 60f;
    [SerializeField]
    private float _moveSpeedMinZoom = 400f;
    [SerializeField]
    private float _moveSpeedMaxZoom = 100f;
    
    [SerializeField]
    private float _rotationSpeedKeyboard = 150f;
    [SerializeField]
    private float _rotationSpeedTouch = 1.5f;
    private Transform _swivel, _stick;
    private static float _zoom = 0f;
    
    
    
   // [SerializeField]
   // private PostProcessingProfile _profile;
   // private PostProcessingBehaviour _postProcessingBehaviour;
    private GameObject _cameraObject;
    
   
  

    private void CameraFree()
    {

        
            float xDelta = 0f, zDelta = 0f, rotationDelta = 0f;

            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0f) AdjustZoom(zoomDelta);

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - _scrollBorderThickness) zDelta++;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.mousePosition.y <= _scrollBorderThickness) zDelta--;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - _scrollBorderThickness) xDelta++;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - _scrollBorderThickness) xDelta++;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.mousePosition.x <= _scrollBorderThickness) xDelta--;
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Comma)) rotationDelta++;
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Period)) rotationDelta--;

            //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) zDelta++;
            //if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) zDelta--;
            //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) xDelta++;
            //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) xDelta--;

            if (xDelta != 0f || zDelta != 0f) AdjustPositionMouse(xDelta, zDelta);
            if (rotationDelta != 0f) AdjustRotationKeyboard(rotationDelta);
       

    }


    void Start()
    {
        
        _zoom = 0f;
       
        _cameraObject = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        //_postProcessingBehaviour = _cameraObject.GetComponent<PostProcessingBehaviour>();
        _swivel = transform.GetChild(0);
        
        _stick = _swivel.GetChild(0);
        //_stickMinZoom = _stick.position.y;
        _scrollBorderThickness *= Screen.height; // because ScrollBorderThickness should be percentage of screen height 
        
    }



    void LateUpdate()
    {
        CameraFree();
    }
  
    
    void AdjustZoom(float delta)
    {
        _zoom = Mathf.Clamp01(_zoom + delta);

        float distance = Mathf.Lerp(_stickMinZoom, _stickMaxZoom, _zoom);
        _stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(_swivelMinZoom, _swivelMaxZoom, _zoom);
        _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);

      
    }

    void AdjustPositionMouse(float xDelta, float zDelta)
    {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float distance = Mathf.Lerp(_moveSpeedMinZoom, _moveSpeedMaxZoom, _zoom) * Time.deltaTime;
        Vector3 position = transform.localPosition;
        transform.localPosition = position += direction * distance;
        //transform.localPosition = ClampPosition(position);
    }

    

    void AdjustRotationKeyboard(float angle)
    {
        angle *= _rotationSpeedKeyboard * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y + angle, 0f);
        
    }

   

}
