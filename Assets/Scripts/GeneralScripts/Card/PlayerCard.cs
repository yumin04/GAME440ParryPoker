using System.Collections;
using SOFile;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private CardDataSO cardData;
    private int cardId;
    private static readonly int Slice = Shader.PropertyToID("_Slice");
    public void Init(int id)
    {
        cardId = id;
        StartCoroutine(InitRoutine());
    }

    public void Init(CardDataSO cardData)
    {
        this.cardData = cardData;
        UpdateTexture();
    }
    
    // TODO: 이거 필요없음, match 및 animation을 보여주면 hand를 보여주는게 확실하게 나중일것이라는거를 알아서
    private IEnumerator InitRoutine()
    {
        while (CardManager.Instance == null)
            yield return null;

        cardData = CardManager.Instance.GetCardByID(cardId);
        UpdateTexture();
    }
    private void UpdateTexture() {
        var propertyBlock = new MaterialPropertyBlock();
        // hack for managing the suit values, some changes will have to be made I'm sure

        propertyBlock.SetFloat(Slice, (int)cardData.cardSymbol * 13 + (cardData.cardNumber - 1));
        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }
}
