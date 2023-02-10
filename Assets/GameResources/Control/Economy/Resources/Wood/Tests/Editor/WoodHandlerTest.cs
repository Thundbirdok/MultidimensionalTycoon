using FluentAssertions;
using NUnit.Framework;

namespace GameResources.Control.Economy.Resources.Wood.Tests.Editor
{
    public class WoodHandlerTest
    {
        [Test]
        public void WhenAddValue_AndValueIsZero_ThenValueShouldBeEqualToAdded()
        {
            // Arrange.
            var handler = new WoodResourceHandler();

            // Act.
            
            handler.ChangeWithoutNotify(0);
            
            handler.Add(10);

            // Assert.

            handler.Value.Should().Be(10);
        }
        
        [Test]
        public void WhenAddValue_AndNoMatterValue_ThenOnValueChangedEventInvoke()
        {
            // Arrange.
            var handler = new WoodResourceHandler();

            var onChangedCount = 0;

            // Act.
            
            handler.ChangeWithoutNotify(0);

            handler.OnValueChanged += OnValueChanged;
            
            handler.Add(10);

            handler.OnValueChanged -= OnValueChanged;

            // Assert.
            
            onChangedCount.Should().Be(1);

            void OnValueChanged()
            {
                ++onChangedCount;
            }
        }
        
        [Test]
        public void WhenSpend1_AndValueIs2_ThenValueShouldBeEqualTo1()
        {
            // Arrange.
            var handler = new WoodResourceHandler();

            // Act.
            
            handler.ChangeWithoutNotify(2);
            
            handler.Spend(1);

            // Assert.

            handler.Value.Should().Be(1);
        }
        
        [Test]
        public void WhenSpend2_AndValueIs1_ThenOnNotEnoughInvoked()
        {
            // Arrange.
            var handler = new WoodResourceHandler();

            var onNotEnoughCount = 0;

            // Act.
            
            handler.ChangeWithoutNotify(1);

            handler.OnNotEnough += OnNotEnough;
            
            handler.Spend(2);

            handler.OnNotEnough -= OnNotEnough;

            // Assert.
            
            onNotEnoughCount.Should().Be(1);

            void OnNotEnough()
            {
                ++onNotEnoughCount;
            }
        }
        
        [Test]
        public void WhenSpend1_AndValueIs2_ThenOnNotEnoughIsNotInvoked()
        {
            // Arrange.
            var handler = new WoodResourceHandler();

            var onNotEnoughCount = 0;

            // Act.
            
            handler.ChangeWithoutNotify(2);

            handler.OnNotEnough += OnNotEnough;
            
            handler.Spend(1);

            handler.OnNotEnough -= OnNotEnough;

            // Assert.
            
            onNotEnoughCount.Should().Be(0);

            void OnNotEnough()
            {
                ++onNotEnoughCount;
            }
        }
    }
}
