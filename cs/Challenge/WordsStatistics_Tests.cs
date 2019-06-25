using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Challenge
{
	[TestFixture]
	public class WordsStatistics_Tests
	{
		public virtual IWordsStatistics CreateStatistics()
		{
			// меняется на разные реализации при запуске exe
			return new WordsStatistics();
		}

		private IWordsStatistics wordsStatistics;

		[SetUp]
		public void SetUp()
		{
			wordsStatistics = CreateStatistics();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterCreation()
		{
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_ContainsItem_AfterAddition()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("def");
			wordsStatistics.GetStatistics().Should().HaveCount(2);
		}

		[Test]
		public void TestUpperAndLowerCase()
		{
			wordsStatistics.AddWord("ABC");
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 2));
		}

		[Test]
		public void TestTruncateWord()
		{
			wordsStatistics.AddWord("12345678901234567890");
			wordsStatistics.AddWord("1234567890");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("1234567890", 2));
		}

		[Test]
		public void TestNullWord()
		{
			
			Action e = () => wordsStatistics.AddWord(null);
			e.ShouldThrow<ArgumentNullException>();
		}

		[Test]
		public void TestWhiteSpaces()
		{
			wordsStatistics.AddWord("  ");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void TestEmptyString()
		{
			wordsStatistics.AddWord("");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}
		
		[Test]
		public void TestStringWithDelimeters()
		{
			wordsStatistics.AddWord("\n\t \n\t");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatisticOrderTest()
		{
			
			wordsStatistics.AddWord("c");
				
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("a");
			
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("b");
			
			
			wordsStatistics.GetStatistics().Should().Equal(new[]
			{
				new WordCount("a", 3),
				new WordCount("b", 2),
				new WordCount("c", 1)
			});
		}

		[Test]
		public void GetStatisticOrderTest2()
		{
			
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("a");
			
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("b");
			
			wordsStatistics.GetStatistics().Should().Equal(new[]
			{
				new WordCount("a", 2),
				new WordCount("b", 2),
			});
		}

		[Test]
		public void GetStatisticOrderTest3()
		{
			wordsStatistics.AddWord("c");
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("b");
			wordsStatistics.GetStatistics().Should().Equal(new[]
			{
				new WordCount("a", 2),
				new WordCount("b", 2),
				new WordCount("c", 1),
			});
		}

		[Test, Timeout(1000)]
		public void TestWorkWithO1()
		{
			for (var i = 0; i < 100000; i++)
			{
				wordsStatistics.AddWord(i.ToString());
			}
		}

		[Test]
		public void TestRussianB()
		{
			wordsStatistics.AddWord("Б");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("б", 1));
		}

		[Ignore("To slow")]
		[Test]
		public void ManyAAndBWordsTest()
		{
			for (int i = 0; i < 1000000; i++)
			{
				wordsStatistics.AddWord("b");
				wordsStatistics.AddWord("a");
			}

			wordsStatistics.GetStatistics().Should().Equal(new[]
			{
				new WordCount("a", 1000000),
				new WordCount("b", 1000000),
			});
		}

		[Test]
		public void TestWordWithDelimeterInside()
		{
			wordsStatistics.AddWord("a b");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("a b", 1));
		}

		[Test]
		public void TestWorkCorrectAfterGetStatistic()
		{
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("a");
			wordsStatistics.GetStatistics();
			wordsStatistics.AddWord("a");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("a", 3));
		}

		[Test]
		public void TestEnumeratorsShouldBeSame()
		{
			wordsStatistics.AddWord("a");
			var firstEnumerator = wordsStatistics.GetStatistics();
			wordsStatistics.AddWord("a");
			var secondEnumerator = wordsStatistics.GetStatistics();
			firstEnumerator.Should().Equal(secondEnumerator);
		}

		[Test]
		public void EveryTimeNewWordCount()
		{
			wordsStatistics.AddWord("a");
			var firstWord = wordsStatistics.GetStatistics().First();
			var secondWord = wordsStatistics.GetStatistics().First();
			firstWord.Should().NotBeSameAs(secondWord);
		}

		[Test]
		public void TestFirstTenSpaces()
		{
			wordsStatistics.AddWord("          123");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("          ", 1));

		}

		[Test]
		public void TestShouldCutElevenSymbols()
		{
			wordsStatistics.AddWord("12345678901");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("1234567890", 1));
		}

		[Test]
		public void TestGetStatisticOrder()
		{
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("b");

			wordsStatistics.GetStatistics().Should().Equal(new[]
			{
				new WordCount("b", 2),
				new WordCount("a", 1),

			});
		}

		[Test]
		public void TestWordStatisticMethodSTA()
		{
			var secondStatistics = CreateStatistics();
			secondStatistics.AddWord("a");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test, Timeout(1000)]
		public void TestWriteManyUniqueWords()
		{
			for (var i = 0; i < 100000; i++)
			{
				wordsStatistics.AddWord(i.ToString());
			}

			wordsStatistics.GetStatistics().Should().HaveCount(100000);
		}

		[Test, Timeout(1000)]
		public void TestManySameWords()
		{
			for (var i = 0; i < 1000; i++)
			{
				for (var j = 0; j < 1000; j++)
				{
					wordsStatistics.AddWord(j.ToString());	
				}
			}

		}


		// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki
	}
}