using System.Collections.Generic;
using System.Linq;
using BlockSystem.Block;
using Other;
using UnityEngine;

namespace BlockSystem
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<BlockType, BlockFactory> blockFactories;

        public static BlockSpawner Instance { get; private set; }

        private void Awake()
        {
            SetInstance();
            InitializeFactories();
        }

        private void InitializeFactories()
        {
            foreach (var blockFactory in blockFactories)
            {
                blockFactory.Value.Initialize();
            }
        }

        private void SetInstance()
        {
            Instance = this;
        }

        public IBlock GetBlock(BlockType blockType)
        {
            if (!blockFactories.ContainsKey(blockType)) return null;
            var block = blockFactories[blockType].GetProduct() as IBlock;
            return block;
        }

        public Dictionary<BlockType, IBlock> SpawnRequestedBlocks(List<BlockType> requestedBlocks)
        {
            return requestedBlocks.Where(requestedBlock => blockFactories.ContainsKey(requestedBlock)).
                ToDictionary(requestedBlock => requestedBlock, GetBlock);
        }
    }
}