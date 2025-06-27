using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.Manager;
using GameModels.Model.Buff;
using NUnit.Framework;

namespace GameLogic.Test
{
    [TestFixture]
    public class BuffManagerTest
    {
        private static BuffManager GetBuffManager()
        {
            return new BuffManager();
        }

        public static IEnumerable<TestCaseData> Cases
        {
            get
            {
                yield return new TestCaseData(GetBuffManager(), new FreezeCropsGrowth()).SetName("FreezeCropsGrowth");
            }
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void AddBuff_VerifyBuffExistingWithCustomAvailable_True(BuffManager buffManager, IBuff buff)
        {
            var expected = true;
            buffManager.AddBuff(buff, () => expected);
            Assert.That(buffManager.Buffs.Any( b => b.Buff == buff), Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void AddBuff_VerifyBuffExistingWithCustomAvailable_False(BuffManager buffManager, IBuff buff)
        {
            var expected = false;
            buffManager.AddBuff(buff, () => expected);
            Assert.That(buffManager.Buffs.Any(b => b.Buff == buff), Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void AddBuff_VerifyBuffExistingWithDefaultAvailable_True(BuffManager buffManager, IBuff buff)
        {
            buffManager.AddBuff(buff, null);
            Assert.That(buffManager.Buffs.Any(b => b.Buff == buff), Is.True);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void AddBuff_VerifyBuffState(BuffManager buffManager, IBuff buff)
        {
            buffManager.AddBuff(buff, null);
            var trackedBuff = buffManager.Buffs.FirstOrDefault(b => b.Buff == buff);
            Assert.That(trackedBuff?.FullState, Is.EqualTo(buff.MaxBuffTurns));
            Assert.That(trackedBuff?.CurrentState, Is.EqualTo(0));
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void IncreaseTurns_VerifyCurrentStateWithDefaultAvailable(BuffManager buffManager, IBuff buff)
        {
            var maxTurns = buff.MaxBuffTurns;
            buffManager.AddBuff(buff, null);
            var trackedBuff = buffManager.Buffs.FirstOrDefault(b => b.Buff == buff);
            for (int i = 1; i <= maxTurns + 1; i++)
            {
                buffManager.IncreaseTurns();
                Assert.That(trackedBuff?.CurrentState, Is.EqualTo(i));
            }
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void IncreaseTurns_ShouldMakeBuffBecomeNotAvailable_True(BuffManager buffManager, IBuff buff)
        {   
            var maxTurns = buff.MaxBuffTurns;
            buffManager.AddBuff(buff, null);
            var trackedBuff = buffManager.Buffs.FirstOrDefault(b => b.Buff == buff);
            for (int i = 1; i <= maxTurns + 1; i++)
            {
                buffManager.IncreaseTurns();
            }

            Assert.That(trackedBuff.IsAvailable, Is.False);
            Assert.That(buffManager.Buffs.Any, Is.False);
        }
    }
}
