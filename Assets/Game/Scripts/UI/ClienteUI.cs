using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClienteUI : MonoBehaviour
{
    // Pedido do cliente
    [SerializeField] GameObject _orderBalloon;
    [SerializeField] GameObject _orderPlaceForItems;
    private int _itemAmount = 0;

    // Espera do cliente
    [SerializeField] private Image _waitImage;

    #region Client order
    public void AddItemToOrder(Sprite sprite)
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.transform.SetParent(_orderPlaceForItems.transform);
        _itemAmount++;
    }

    private void ResetItems()
    {
        for(int i = 0; i < _itemAmount; i++)
        {
            Destroy(_orderPlaceForItems.transform.GetChild(0));
        }
        _itemAmount = 0;
    }
    #endregion

    #region Client wait
    public void TurnOnWait()
    {
        _waitImage.gameObject.SetActive(true);
        _waitImage.fillAmount = 1;
    }

    public void UpdateWaitSprite(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;
    }


    #endregion
}
