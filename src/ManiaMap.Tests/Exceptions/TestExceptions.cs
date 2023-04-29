using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Exceptions.Tests
{
    [TestClass]
    public class TestExceptions
    {
        [TestMethod]
        public void TestThrowExceptions()
        {
            Assert.ThrowsException<CellsNotFullyConnectedException>(() => throw new CellsNotFullyConnectedException("Test"));
            Assert.ThrowsException<CollectableSpotNotFoundException>(() => throw new CollectableSpotNotFoundException("Test"));
            Assert.ThrowsException<DuplicateIdException>(() => throw new DuplicateIdException("Test"));
            Assert.ThrowsException<EmptyGraphException>(() => throw new EmptyGraphException("Test"));
            Assert.ThrowsException<GraphNotFullyConnectedException>(() => throw new GraphNotFullyConnectedException("Test"));
            Assert.ThrowsException<InvalidChainOrderException>(() => throw new InvalidChainOrderException("Test"));
            Assert.ThrowsException<InvalidIdException>(() => throw new InvalidIdException("Test"));
            Assert.ThrowsException<InvalidNameException>(() => throw new InvalidNameException("Test"));
            Assert.ThrowsException<NoDoorsExistException>(() => throw new NoDoorsExistException("Test"));
            Assert.ThrowsException<NoTemplateGroupAssignedException>(() => throw new NoTemplateGroupAssignedException("Test"));
            Assert.ThrowsException<UnhandledCaseException>(() => throw new UnhandledCaseException("Test"));
        }
    }
}