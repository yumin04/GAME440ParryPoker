using System;
using System.Collections.Generic;
using System.Linq;
using GenericHelpers;
using SOFile;

public class PokerHandCalculator
{
    private List<CardData> currentHandData;
    // Array.Sort(handData, (a, b) => a.cardNumber.CompareTo(b.cardNumber));
    // How to sort things
    public PokerHandCalculator()
    {
        currentHandData = new List<CardData>();
    }
    
    // TODO: USED IN UNITY
    public int GetPokerResult(CardDataSO[] handData)
    {
        CardData[] converted = new CardData[handData.Length];
        for (int i = 0; i < handData.Length; i++)
        {
            converted[i].cardID = handData[i].cardID;
            converted[i].cardNumber = handData[i].cardNumber;
            converted[i].cardSymbol = handData[i].cardSymbol;
        }
        return GetPokerResult(converted);
    }
    
    public int GetPokerResult(CardData[] handData)
    {
        HandRank rank = CalculatePokerHand(handData);
        FindBestHand(rank, handData);
        return 1;
    }

    private void FindBestHand(HandRank rank, CardData[] handData)
    {
        
        switch (rank)
        {
            case HandRank.StraightFlush:
                break;
            case HandRank.FourOfAKind:
                break;
            case HandRank.FullHouse:
                break;
            case HandRank.Flush:
                break;
            case HandRank.Straight:
                break;
            case HandRank.ThreeOfAKind:
                break;
            case HandRank.TwoPair:
                break;
            case HandRank.OnePair:
                break;
            case HandRank.HighCard:
                break;
            
        }
        
    }


    public HandRank CalculatePokerHand(CardData[] handData)
    {
        if (CheckStraightFlush(handData))
            return HandRank.StraightFlush;
        
        if(CheckFourOfAKind(handData))
            return HandRank.FourOfAKind;
        
        if(CheckFullHouse(handData))
            return HandRank.FullHouse;
        
        if (CheckFlush(handData))
            return HandRank.Flush;
        
        if(CheckStraight(handData))
            return HandRank.Straight;
        
        if(CheckThreeOfAKind(handData))
            return HandRank.ThreeOfAKind;
        
        if (CheckTwoPair(handData))
            return HandRank.TwoPair;
        
        if(CheckOnePair(handData))
            return HandRank.OnePair;
        
        return HandRank.HighCard;
    }

    public bool CheckStraightFlush(CardData[] handData)
    {
        var suitGroups = new Dictionary<Suit, List<CardData>>();

        foreach (var card in handData)
        {
            if (!suitGroups.ContainsKey(card.cardSymbol))
                suitGroups[card.cardSymbol] = new List<CardData>();

            suitGroups[card.cardSymbol].Add(card);
        }

        foreach (var group in suitGroups.Values)
        {
            if (group.Count >= 5 && CheckStraight(group.ToArray()))
                return true;
        }

        return false;
    }

    public bool CheckFourOfAKind(CardData[] handData)
    {
        var count = new Dictionary<int, int>();

        foreach (var card in handData)
        {
            if (!count.ContainsKey(card.cardNumber))
                count[card.cardNumber] = 0;

            count[card.cardNumber]++;

            if (count[card.cardNumber] >= 4)
                return true;
        }

        return false;
    }
    public bool CheckFullHouse(CardData[] handData)
    {
        var count = new Dictionary<int, int>();

        foreach (var card in handData)
        {
            if (!count.ContainsKey(card.cardNumber))
                count[card.cardNumber] = 0;

            count[card.cardNumber]++;
        }

        bool hasThree = false;
        bool hasTwo = false;

        foreach (var kvp in count)
        {
            if (kvp.Value >= 3)
                hasThree = true;
            else if (kvp.Value >= 2)
                hasTwo = true;
        }

        return hasThree && hasTwo;
    }
    public bool CheckFlush(CardData[] handData)
    {
        var suitCount = new Dictionary<Suit, int>();

        foreach (var card in handData)
        {
            if (!suitCount.ContainsKey(card.cardSymbol))
                suitCount[card.cardSymbol] = 0;

            suitCount[card.cardSymbol]++;

            if (suitCount[card.cardSymbol] >= 5)
                return true;
        }
        return false;
    }
    public bool CheckStraight(CardData[] handData)
    {
        var numbers = new HashSet<int>();

        foreach (var card in handData)
        {
            numbers.Add(card.cardNumber);
        }

        var sorted = numbers.OrderByDescending(n => n).ToList();
        
        for (int i = 0; i <= sorted.Count - 5; i++)
        {
            if (sorted[i] - sorted[i + 4] == 4)
                return true;
        }
        return false;
    }
    public bool CheckThreeOfAKind(CardData[] handData)
    {
        var count = new Dictionary<int, int>();

        foreach (var card in handData)
        {
            if (!count.ContainsKey(card.cardNumber))
                count[card.cardNumber] = 0;

            count[card.cardNumber]++;

            if (count[card.cardNumber] >= 3)
                return true;
        }

        return false;
    }
    public bool CheckTwoPair(CardData[] handData)
    {
        var count = new Dictionary<int, int>();
        int pairCount = 0;

        foreach (var card in handData)
        {
            if (!count.ContainsKey(card.cardNumber))
                count[card.cardNumber] = 0;

            count[card.cardNumber]++;
        }

        foreach (var kvp in count)
        {
            if (kvp.Value >= 2)
                pairCount++;
        }

        return pairCount >= 2;
    }
    public bool CheckOnePair(CardData[] handData)
    {
        var count = new Dictionary<int, int>();

        foreach (var card in handData)
        {
            if (!count.ContainsKey(card.cardNumber))
                count[card.cardNumber] = 0;

            count[card.cardNumber]++;

            if (count[card.cardNumber] >= 2)
                return true;
        }

        return false;
    }
    public bool CheckHighCard(CardData[] handData)
    {
        return !CheckOnePair(handData)
               && !CheckTwoPair(handData)
               && !CheckThreeOfAKind(handData);
    }
}