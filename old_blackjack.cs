using System;
using System.Collections.Generic;
using System.Threading;

class Blackjack
{
  static List<string> deck = new List<string>(); // The deck used in game (empty)
  static string[] fullDeck = {"AoD", "2oD", "3oD", "4oD", "5oD", "6oD", "7oD", "8oD", "9oD", "10oD", // Contains every card in
  "JoD", "QoD", "KoD", "AoH", "2oH", "3oH", "4oH", "5oH", "6oH", "7oH", "8oH", "9oH", "10oH", "JoH", // a regular deck of cards
  "QoH", "KoH", "AoC", "2oC", "3oC", "4oC", "5oC", "6oC", "7oC", "8oC", "9oC", "10oC", "JoC", "QoC", // (no jokers)
  "KoC", "AoS", "2oS", "3oS", "4oS", "5oS", "6oS", "7oS", "8oS", "9oS", "10oS", "JoS", "QoS", "KoS"};
  static List<string> yrCards = new List<string>(); // The cards on your hand (empty)
  static List<string> dlCards = new List<string>(); // The cards on the dealer's hand (empty)
  static string youGet;
  static string dlrGet;
  static bool youBst = false;
  static bool dlrBst = false;
  
  static void Main() // Starting point - Can be used to reset the deck[]
  {
    Console.Clear();
    yrCards.Clear();
    dlCards.Clear();
    deck.Clear();
    deck.AddRange(fullDeck);
    Program();
  }
  
  static void Program()
  {
    Deal(); // Line 164

    Thread.Sleep(2000); // Shows the cards for 2000 ms before doing anything

    bool bj = false;
    if(TotalValue(yrCards) == 21 || TotalValue(dlCards) == 21) // Checks if someone got blackjack
    {
      if(TotalValue(yrCards) == 21) // The player got blackjack
      {
        Console.Clear();
        Console.WriteLine("\n <----YOU GOT---->\n <**BLACKJACK**>\n");
        bj = true;
      }

      if(TotalValue(dlCards) == 21) // The dealer got blackjack
      {
        if(!bj) // Only the dealer got blackjack
        {
          Console.Clear();
          Console.WriteLine("\n <-THE DEALER GOT->\n  <**BLACKJACK**>\n");
          youGet = "nothing";
          dlrGet = "your bet";
        }
        else // Both got blackjack
        {
          Console.WriteLine("\n <-THE DEALER GOT->\n  <**BLACKJACK**>\n");
          youGet = "your bet back";
          dlrGet = "nothing";
        }
      }
      else // Only the player got blackjack
      {
        youGet = "your bet back plus 1.5x your bet";
        dlrGet = "nothing";
      }
    }
    else // No one got blackjack
    {
      CheckIfBust();

      if(youBst && dlrBst)
      {
        Console.Clear();
        Console.WriteLine("\n <----YOU GOT---->\n <**BUST**>\n");
        Console.WriteLine("\n <-THE DEALER GOT->\n <**BUST**>\n");
        youGet = "nothing";
        dlrGet = "your bet";
      }
      else if(youBst)
      {
        Console.Clear();
        yrCards.Clear();
        yrCards.Add("<**BUST**>");
        ShowCards();
        Thread.Sleep(1500);
        DealerPlay(); // player got bust, dealer plays
      }
      else if(dlrBst)
      {
        Console.Clear();
        dlCards.Clear();
        dlCards.Add("<**BUST**>");
        ShowCards();
        Thread.Sleep(1500);
        PlayerPlay(false); // dealer got bust, player plays
      }
      else
      {
        PlayerPlay(true); // no one got bust, both plays
      }

      if(youBst && dlrBst)
      {
        Console.Clear();
        yrCards.Clear();
        yrCards.Add("<**BUST**>");
        dlCards.Clear();
        dlCards.Add("<**BUST**>");
        ShowCards();
        Thread.Sleep(1500);
        youGet = "nothing";
        dlrGet = "your bet";
      }
      else if(youBst)
      {
        youGet = "nothing";
        dlrGet = "your bet";
      }
      else if(dlrBst)
      {
        youGet = "your bet back plus 1x your bet";
        dlrGet = "nothing";
      }
      else
      {
        Console.Clear();
        Console.WriteLine("\n You got a hand worth " + TotalValue(yrCards));
        Console.WriteLine("\n The dealer got a hand worth " + TotalValue(dlCards));
        if(TotalValue(yrCards) > TotalValue(dlCards))
        {
          Console.WriteLine("\n <----YOU WON---->");
          youGet = "your bet back plus 1x your bet";
          dlrGet = "nothing";
        }
        else if(TotalValue(dlCards) > TotalValue(yrCards))
        {
          Console.WriteLine("\n <-THE DEALER WON->");
          youGet = "nothing";
          dlrGet = "your bet";
        }
        else
        {
          Console.WriteLine("\n <---IT'S EVEN--->");
          youGet = "your bet back";
          dlrGet = "nothing";
        }
      }
    }
    Console.WriteLine("\n\n  You get " + youGet);
    Console.WriteLine("\n  The dealer gets " + dlrGet);
    Console.WriteLine("\n\n *--------------------------*");
    Console.WriteLine("  To play again, press Enter\n\n  To exit, press any key");
    Console.WriteLine(" *--------------------------*");

    var k = Console.ReadKey().Key;

    if(k == ConsoleKey.Enter)
    {
      Main();
    }
  }
  
  static void Deal() // Adds two cards to the player's hand
  {                  // and two cards to the dealer's hand
    Console.WriteLine("\n Dealing...");

    yrCards.Add(DrawCard()); // Waits 500 ms to make it more
    Thread.Sleep(500);       // random, because Random.Next
    dlCards.Add(DrawCard()); // is only pseudo random
    Thread.Sleep(500);
    yrCards.Add(DrawCard());
    Thread.Sleep(500);
    dlCards.Add(DrawCard());

    ShowCards(); // Line 206
  }
  
  static string DrawCard() // Gets a random card from the deck[]
  {                        // removes it, and then returns it
    var rnd = new Random();
    int rndIndex = rnd.Next(deck.Count);
    string card = deck[rndIndex];
    deck.Remove(card);
    return card;
  }
    
  static int ToValue(string x)
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
      	value = int.Parse(x[0].ToString());
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

  static void ShowCards() // Displays the cards like in the example below
  {
    Console.Clear();
    Console.WriteLine("\n Your cards:\n  " + string.Join("\n  ", yrCards.ToArray()) +
    "\n The dealer's cards\n  " + string.Join("\n  ", dlCards.ToArray()));
  }
  /*
   Your cards:
    8oD
    KoH
   The dealer's cards:
    4oC
    7oH
  */

  static int TotalValue(List<string> listOfCards)
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

  static void CheckIfBust()
  {
    if(TotalValue(yrCards) >= 22) // The player got bust
    {
      youBst = true;
    }

    if(TotalValue(dlCards) == 21) // The dealer got bust
    {
      dlrBst = true;
    }

  }

  static void PlayerPlay(bool both)
  {
    bool a = true;
    while(a)
    {
      Console.WriteLine("\n\nCard - Press C\nStop - Press S");

      var pressedKey = Console.ReadKey(true).Key;
      if(pressedKey == ConsoleKey.C)
      {
        yrCards.Add(DrawCard());
        ShowCards();
        Thread.Sleep(1500);
        if(TotalValue(yrCards) >= 22)
        {
          Console.Clear();
          Console.WriteLine("\n  <----YOU GOT---->\n <**BUST**>\n");
          yrCards.Clear();
          yrCards.Add("<**BUST**>");
          youBst = true;
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

  static void DealerPlay()
  {
  	Console.Clear();
    while(TotalValue(dlCards) <= 16)
    {
      dlCards.Add(DrawCard());
      ShowCards();
      Thread.Sleep(1500);
    }
    ShowCards();
    Thread.Sleep(2000);
    if(TotalValue(dlCards) >= 22)
    {
      Console.Clear();
      Console.WriteLine("\n <-THE DEALER GOT->\n  <**BUST**>\n");
      dlCards.Clear();
      dlCards.Add("<**BUST**>");
      dlrBst = true;
    }
  }
}