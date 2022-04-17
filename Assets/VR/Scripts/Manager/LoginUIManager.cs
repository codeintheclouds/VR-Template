using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ConnectOptionsPanelGameobject;
    [SerializeField]
    private GameObject ConnectWithNamePanelGameobject;

    #region Unity Methods

    void Start()
    {
        ConnectOptionsPanelGameobject.SetActive(true);
        ConnectWithNamePanelGameobject.SetActive(false);
    }  

    #endregion
}
