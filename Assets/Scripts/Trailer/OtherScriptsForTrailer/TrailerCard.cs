using System.Collections;
using SOFile;
using UnityEngine;

public class TrailerCard : MonoBehaviour
{
    [SerializeField] private CardDataSO cardData;
    private int cardId;
    private static readonly int Slice = Shader.PropertyToID("_Slice");
    public void Init(CardDataSO _cardData)
    {
        this.cardData = _cardData ;
        UpdateTexture();
    }
    
    private void UpdateTexture() {
        var propertyBlock = new MaterialPropertyBlock();
        // hack for managing the suit values, some changes will have to be made I'm sure

        propertyBlock.SetFloat(Slice, (int)cardData.cardSymbol * 13 + (cardData.cardNumber - 1));


        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }
}