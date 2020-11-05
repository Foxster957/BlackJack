using System;
using System.Collections.Generic;
using System.Threading;

public class Blackjack
{
  public static List<string> deck = new List<string>(); // The deck used in game (empty)
  static string[] fullDeck = {"AoD", "2oD", "3oD", "4oD", "5oD", "6oD", "7oD", "8oD", "9oD", "10oD", // Contains every card in
  "JoD", "QoD", "KoD", "AoH", "2oH", "3oH", "4oH", "5oH", "6oH", "7oH", "8oH", "9oH", "10oH", "JoH", // a regular deck of cards
  "QoH", "KoH", "AoC", "2oC", "3oC", "4oC", "5oC", "6oC", "7oC", "8oC", "9oC", "10oC", "JoC", "QoC", // (no jokers)
  "KoC", "AoS", "2oS", "3oS", "4oS", "5oS", "6oS", "7oS", "8oS", "9oS", "10oS", "JoS", "QoS", "KoS"};
  public static List<string> yrCards = new List<string>(); // The cards on your hand (empty)
  public static List<string> dlCards = new List<string>(); // The cards on the dealer's hand (empty)
  public static string youGet;
  public static string dlrGet;
  public static bool youBst = false;
  public static bool dlrBst = false;
  
  static void Main() // Starting point - Can be used to reset the deck[]
  {
    for( ; ; )
    {
      Console.Clear();
      yrCards.Clear();
      dlCards.Clear();
      deck.Clear();
      deck.AddRange(fullDeck);
      Program();
  	}
  }
  
  static void Program()
  {
  	Betting.PlaceBet();
    Methods.Deal();

    Thread.Sleep(3000); // Shows the cards for 3000 ms before doing anything

    bool bj = false;
    if(Methods.TotalValue(yrCards) == 21 || Methods.TotalValue(dlCards) == 21) // Checks if someone got blackjack
    {
      if(Methods.TotalValue(yrCards) == 21) // The player got blackjack
      {
        Console.WriteLine("\n <----YOU GOT---->\n <**BLACKJACK**>\n");
        bj = true;
      }

      if(Methods.TotalValue(dlCards) == 21) // The dealer got blackjack
      {
        if(!bj) // Only the dealer got blackjack
        {
          Console.WriteLine("\n <-THE DEALER GOT->\n  <**BLACKJACK**>\n");
          youGet = "lose your bet";
          dlrGet = "get " + Betting.bet;
        }
        else // Both got blackjack
        {
          Console.WriteLine("\n <-THE DEALER GOT->\n  <**BLACKJACK**>\n");
          youGet = "get your bet back (" + Betting.bet + ")";
          dlrGet = "gets nothing";
        }
      }
      else // Only the player got blackjack
      {
        youGet = "get your bet back (" + Betting.bet + ") plus 1.5x your bet (" + Betting.bet*1.5 + ")";
        dlrGet = "gets nothing";
      }
    }
    else // No one got blackjack
    {
      Methods.CheckIfBust();

      if(youBst && dlrBst)
      {
        Console.WriteLine("\n <----YOU GOT---->\n <**BUST**>\n");
        Console.WriteLine("\n <-THE DEALER GOT->\n <**BUST**>\n");
        youGet = "lose your bet";
        dlrGet = "gets " + Betting.bet;
      }
      else if(youBst)
      {
        Console.Clear();
        yrCards.Clear();
        yrCards.Add("<**BUST**>");
        Methods.ShowCards();
        Thread.Sleep(1500);
        Methods.DealerPlay(); // player got bust, dealer plays
      }
      else if(dlrBst)
      {
        Console.Clear();
        dlCards.Clear();
        dlCards.Add("<**BUST**>");
        Methods.ShowCards();
        Thread.Sleep(1500);
        Methods.PlayerPlay(false); // dealer got bust, player plays
      }
      else
      {
        Methods.PlayerPlay(true); // no one got bust, both plays
      }

      if(youBst && dlrBst)
      {
        Console.Clear();
        yrCards.Clear();
        yrCards.Add("<**BUST**>");
        dlCards.Clear();
        dlCards.Add("<**BUST**>");
        Methods.ShowCards();
        Thread.Sleep(1500);
        youGet = "lose your bet";
        dlrGet = "gets " + Betting.bet;
      }
      else if(youBst)
      {
        youGet = "lose your bet";
        dlrGet = "get " + Betting.bet;
      }
      else if(dlrBst)
      {
        youGet = "get your bet back (" + Betting.bet + ") plus 1x your bet (" + Betting.bet + ")";
        dlrGet = "gets nothing";
      }
      else
      {
        Console.WriteLine("\n You got a hand worth " + Methods.TotalValue(yrCards));
        Console.WriteLine("\n The dealer got a hand worth " + Methods.TotalValue(dlCards));
        if(Methods.TotalValue(yrCards) > Methods.TotalValue(dlCards))
        {
          Console.WriteLine("\n <----YOU WON---->");
          youGet = "get your bet back (" + Betting.bet + ") plus 1x your bet (" + Betting.bet + ")";
          dlrGet = "gets nothing";
        }
        else if(Methods.TotalValue(dlCards) > Methods.TotalValue(yrCards))
        {
          Console.WriteLine("\n <-THE DEALER WON->");
          youGet = "get nothing";
          dlrGet = "gets " + Betting.bet;
        }
        else
        {
          Console.WriteLine("\n <---IT'S EVEN--->");
          youGet = "get your bet back (" + Betting.bet + ")";
          dlrGet = "gets nothing";
        }
      }
    }
    Betting.Payout();

    Console.WriteLine("\n\n Press any key to play again...");

    Console.ReadKey();
  }
}

public static class Methods
{
  public static void Deal() // Adds two cards to the player's hand
  {                  // and two cards to the dealer's hand
  	Console.WriteLine("\n Dealing...");

    Blackjack.yrCards.Add(DrawCard()); // Waits 500 ms to make it more
    Thread.Sleep(500);       // random, because Random.Next
    Blackjack.dlCards.Add(DrawCard()); // is only pseudo random
    Thread.Sleep(500);
    Blackjack.yrCards.Add(DrawCard());
    Thread.Sleep(500);
    Blackjack.dlCards.Add(DrawCard());

    ShowCards();
  }
  
  public static string DrawCard() // Gets a random card from the deck[]
  {                        // removes it, and then returns it
    var rnd = new Random();
    int rndIndex = rnd.Next(Blackjack.deck.Count);
    string card = Blackjack.deck[rndIndex];
    Blackjack.deck.Remove(card);
    return card;
  }
    
  public static int ToValue(string x)
  {
    int value;

    if(Char.IsDigit(x[0]))
    {
      if(x[0] == '1')
      {
      	value = 10;
      }
      else
      {
      	value = Int32.Parse(x[0].ToString());
      }
    }
    else if(x[0] == 'A')
    {
      value = 1;
    }
    else
    {
      value = 10;
    }

    return value;    
  }

  public static void ShowCards() // Displays the cards like in the example below
  {
    Console.Clear();
    Console.WriteLine("\n Your bet:\n  " + Betting.bet);
    if(Blackjack.youBst)
    {
      Console.WriteLine("\n You are\n  <**BUST**>");
    }
    else
    {
      Console.WriteLine("\n Your cards:\n  " + string.Join("\n  ", Blackjack.yrCards.ToArray()));
    }

    if(Blackjack.dlrBst)
    {
      Console.WriteLine("\n The dealer is\n  <**BUST**>");
    }
    else
    {
      Console.WriteLine("\n The dealer's cards\n  " + string.Join("\n  ", Blackjack.dlCards.ToArray()));
    }
  }

  public static int TotalValue(List<string> listOfCards)
  {
    int aces = 0;
    int result = 0;
    foreach(string card in listOfCards)
    {
      if(ToValue(card) == 1)
      {
        aces += 1;
      }
      result += ToValue(card);
    }
    for( ; aces >= 1; aces -= 1)
    {
      if((result + 10) <= 21)
      {
        result += 10;
      }
    }
    return result;
  }

  public static void CheckIfBust()
  {
    if(TotalValue(Blackjack.yrCards) >= 22) // The player got bust
    {
      Blackjack.youBst = true;
    }
    else
    {
      Blackjack.youBst = false;
    }

    if(TotalValue(Blackjack.dlCards) == 21) // The dealer got bust
    {
      Blackjack.dlrBst = true;
    }
    else
    {
      Blackjack.dlrBst = false;
    }

  }

  public static void PlayerPlay(bool both)
  {
    bool a = true;
    while(a)
    {
      Console.WriteLine("\n\nCard - Press C\nStop - Press S");

      var pressedKey = Console.ReadKey(true).Key;
      if(pressedKey == ConsoleKey.C)
      {
        Blackjack.yrCards.Add(DrawCard());
        ShowCards();
        Thread.Sleep(1500);
        if(TotalValue(Blackjack.yrCards) >= 22)
        {
          Console.Clear();
          Console.WriteLine("\n  <----YOU GOT---->\n <**BUST**>\n");
          Blackjack.yrCards.Clear();
          Blackjack.yrCards.Add("<**BUST**>");
          Blackjack.youBst = true;
          if(both)
          {
            DealerPlay();
            return;
          }
          a = false;
        }
      }
      else if(pressedKey == ConsoleKey.S)
      {
        a = false;
        if(both)
        {
          DealerPlay();
          return;
        }
      }
    }
  }

  public static void DealerPlay()
  {
  	Console.Clear();
    while(TotalValue(Blackjack.dlCards) <= 16)
    {
      Blackjack.dlCards.Add(DrawCard());
      ShowCards();
      Thread.Sleep(1500);
    }
    ShowCards();
    Thread.Sleep(2000);
    if(TotalValue(Blackjack.dlCards) >= 22)
    {
      Console.Clear();
      Console.WriteLine("\n <-THE DEALER GOT->\n  <**BUST**>\n");
      Blackjack.dlCards.Clear();
      Blackjack.dlCards.Add("<**BUST**>");
      Blackjack.dlrBst = true;
    }
  }
}

public static class Betting
{
  public static int bet;
  public static void PlaceBet()
  {
  	int a = 1;
  	while(a == 1)
  	{
  	  Console.Clear();
  	  Console.WriteLine("\n How much do you want to bet?\n");
  	  string input = Console.ReadLine();
  	  if(Int32.TryParse(input, out bet))
  	  {
  	  	Console.Clear();
  	    Console.WriteLine("\n Your bet:\n  " + bet);
  	    Console.WriteLine("\n To confirm, press Enter\n To change, press any key");
  	    var k = Console.ReadKey().Key;
  	    if(k == ConsoleKey.Enter)
  	    {
  	  	  a = 0;
  	    }
  	  }
  	  else
  	  {
  	  	Console.Clear();
  	  	Console.WriteLine("ERROR: Invalid input (NaN)");
  	  	Console.Beep(800, 200);
  	  	Console.Beep(700, 200);
  	  	Thread.Sleep(1600);
  	  }
  	}
  }

  public static void Payout()
  {
  	Console.WriteLine("\n\n  You " + Blackjack.youGet);
    Console.WriteLine("\n  The dealer " + Blackjack.dlrGet);
  }
}