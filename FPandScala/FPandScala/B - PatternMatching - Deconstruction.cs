using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace FPandScala
{
    public class TradeClass
    {
        public string TradeId { get; set; }
        public decimal Amount { get; set; }

        public void Deconstruct(out string tradeId, out decimal amount)
        {
            tradeId = TradeId;
            amount = Amount;
        }
    }

    // Records automatically add Deconstructors 'matching' the default constructor parameter order
    public record TradeRecord(string TradeId, decimal Amount);

    [TestFixture]
    public class B_PatternMatching_Deconstruction
    {
        [Test]
        public void DeconstructClass()
        {
            var trade = new TradeClass
            {
                Amount = 123.45m,
                TradeId = "998765"
            };

            // Note that this isn't actually a tuple. It's deconstruction ...
            var (tradeId, amount) = trade;

            Assert.AreEqual(trade.Amount, amount);
            Assert.AreEqual(trade.TradeId, tradeId);
        }

        // With records, we get deconstruction for free...
        [Test]
        public void DeconstructRecord()
        {
            var trade = new TradeRecord("998765", 123.45m);

            var (tradeId, amount) = trade;

            Assert.AreEqual(trade.Amount, amount);
            Assert.AreEqual(trade.TradeId, tradeId);
        }

        [TestCase("998765", 123.45)]
        [TestCase("123456", 999.45)]
        [TestCase("12321321", 200)]
        [TestCase("33213", 333.45)]
        public void PatternMatchUsingDeconstruction(string tradeId, double amount)
        {
            // Note that decimal is not regarded a compile time constant in .Net (Cast)
            var theTrade = new TradeRecord(tradeId, (decimal)amount);
            var myTradeString = theTrade switch
            {
                var (_, amt) when amt > 500 => "That's a big trade!!",
                var (_, amt) when amt < 100 => "That's a small trade!!",
                // Personally, I don't like the is, or, and DSL
                var (_, amt) when amt is 200 or 250 => "Bingo", //amt == 200 || amt ==  250
                _ => "Just an average trade"
            };
            Console.WriteLine($"{theTrade} => {myTradeString}");
        }

        [TestCase("998765", 123.45)]
        [TestCase("123456", 999.45)]
        [TestCase("33213", 333.45)]
        public void PatternMatchUsingDeconstructionAndRelationals(string tradeId, double amount)
        {
            // Note that decimal is not regarded a compile time constant in .Net (Cast)
            var theTrade = new TradeRecord(tradeId, (decimal)amount);
            var myTradeString = theTrade switch
            {
                (_, > 500m) => "That's a big trade!!",
                (_, < 100) => "That's a small trade!!",
                // Think this is coming in C#12 (_, {200,250}) => "Bingo",
                _ => "Just an average trade"
            };
            Console.WriteLine($"{theTrade} => {myTradeString}");
        }

        [TestCase("998765", 123.45)]
        [TestCase("123456", 999.45)]
        [TestCase("33213", 333.45)]
        public void PatternMatchUsingPropertyPatterns(string tradeId, double amount)
        {
            // Note that decimal is not regarded a compile time constant in .Net (Cast)
            var theTrade = new TradeRecord(tradeId, (decimal)amount);

            var myTradeString = theTrade switch
            {
                // Resharper suggests refactoring .. (show)
                { Amount: > 500 } => "That's a big trade!!",
                { Amount: < 100 } => "That's a small trade!!",
                _ => "Just an average trade"
            };
            Console.WriteLine($"{theTrade} => {myTradeString}");
        }
    }
}
