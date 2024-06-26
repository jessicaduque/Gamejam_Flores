using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClienteUI : MonoBehaviour
{
    // Pedido do cliente
    [SerializeField] GameObject _orderBalloon;
    [SerializeField] GameObject _orderPlaceForItems;
    private List<Image> _currentImages;
    private int _itemAmount = 0;

    [SerializeField] private Color _corVerde;
    [SerializeField] private Color _corAmarelo;
    [SerializeField] private Color _corVermelho;

    // Espera do cliente
    [SerializeField] private Image _waitImage;

    #region Client order
    public void TurnOnOrder()
    {
        _orderBalloon.SetActive(true);
        _currentImages = new List<Image>();
    }
    public void AddItemToOrder(Sprite sprite)
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.transform.SetParent(_orderPlaceForItems.transform);
        _currentImages.Add(image);
        _itemAmount++;
    }
    
    public void RemoveItemSprite(Sprite sprite)
    {
        for(int i=0; i<_itemAmount; i++)
        {
            if(_currentImages[i].sprite == sprite)
            {
                Destroy(_currentImages[i].gameObject);
                _itemAmount--;
                return;
            }
        }
        
    }

    public void TurnOffOrder()
    {
        _orderBalloon.SetActive(false);
        ResetItems();
    }

    private void ResetItems()
    {
        for(int i = 0; i < _currentImages.Count; i++)
        {
            Destroy(_currentImages[i]);
        }
        _itemAmount = 0;
    }
    #endregion

    #region Client wait
    public void TurnOnWait()
    {
        _waitImage.gameObject.SetActive(true);
        _waitImage.fillAmount = 0;
    }

    public void UpdateWaitSprite(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;

        if (fillAmount < 0.33f)
        {
            _waitImage.color = _corVerde;
        }
        else if (fillAmount < 0.66f)
        {
            _waitImage.color = _corAmarelo;
        }
        else
        {
            _waitImage.color = _corVermelho;
        }
    }

    public void TurnOffWait()
    {
        _waitImage.gameObject.SetActive(false);
    }

    #endregion
}
