using Snowcap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Takes care of all pipeline related functionality.
    /// </summary>
    [Serializable]
    public class NavTilePipelineManager
    {
        // Amount of threads along side the Unity main thread.
        public int NumberOfThreads = 1;

        // Enable multi-threading.
        public bool MultiThreadingEnabled = true;

        // List of enabled smoothing algorithms.
        [SerializeField]
        public List<string> EnabledSmoothingAlgorithms = new List<string>();

        /// <summary>
        /// The assembly qualified name of the algorithm class currently used.
        /// Default is A*.
        /// </summary>
        public string AlgorithmType = typeof(AStar).AssemblyQualifiedName;

        // The algorithm instance used to calculate a path.
        private IPathfindingAlgorithm _algorithm;
        public IPathfindingAlgorithm Algorithm
        {
            get
            {
                if (_algorithm == null || !Type.GetType(AlgorithmType).Equals(_algorithm.GetType()))
                {
                    Type algorithm = Type.GetType(AlgorithmType);

                    _algorithm = Activator.CreateInstance(algorithm) as IPathfindingAlgorithm;
                }
                return _algorithm;
            }
        }

        // The Pipeline instance which converts the FindPathInput input to a NavTilePath.
        private Pipeline<FindPathInput, NavTilePath> _pipelineMultiThreaded;

        /// <summary>
        /// Returns a path through the pipeline.
        /// </summary>
        /// <param name="inInput">Input parameters for the path.</param>
        public async Task<NavTilePath> GetPath(FindPathInput inInput)
        {
            if (MultiThreadingEnabled)
            {
                if (_pipelineMultiThreaded == null)
                {
                    CreatePipeline();
                }

                return await _pipelineMultiThreaded.Execute(inInput);
            }
            else
            {
                return DoLinearPipeline(inInput);
            }
        }

        /// <summary>
        /// Initializes the pipeline.
        /// </summary>
        private void CreatePipeline()
        {
            // Create empty pipeline.
            _pipelineMultiThreaded = new Pipeline<FindPathInput, NavTilePath>();

            // Create Pathfinding instance.
            if (string.IsNullOrEmpty(AlgorithmType))
            {
                throw new Exception("Did not set the algorithm");
            }

            Type algorithm = Type.GetType(AlgorithmType);

            _algorithm = Activator.CreateInstance(algorithm) as IPathfindingAlgorithm;

            _pipelineMultiThreaded.AddStep<FindPathInput, NavTilePath>(_algorithm.FindPath, NumberOfThreads);

            List<Type> smoothing = new List<Type>();

            foreach (var smoothingAlgorithm in EnabledSmoothingAlgorithms)
            {
                smoothing.Add(Type.GetType(smoothingAlgorithm));
            }

            foreach (var type in smoothing)
            {
                var instance = Activator.CreateInstance(type) as INavTilePathModifier;
                _pipelineMultiThreaded.AddStep<NavTilePath, NavTilePath>(instance.ModifyPath, 1);
            }
        }

        /// <summary>
        /// Executes the pipeline on the Unity main thread.
        /// </summary>
        /// <param name="inInput">Input parameters for the path.</param>
        private NavTilePath DoLinearPipeline(FindPathInput inInput)
        {
            if (_algorithm == null)
            {
                Type algorithm = Type.GetType(AlgorithmType);
                _algorithm = Activator.CreateInstance(algorithm) as IPathfindingAlgorithm;
            }

            NavTilePath path = _algorithm.FindPath(inInput);

            foreach (var smoothingAlgorithm in EnabledSmoothingAlgorithms)
            {
                var instance = Activator.CreateInstance(Type.GetType(smoothingAlgorithm)) as INavTilePathModifier;
                path = instance.ModifyPath(path);
            }

            return path;
        }

        /// <summary>
        /// Returns a list of types of all classes which implement IPathfindingAlgorithm.
        /// </summary>
        public List<Type> GetPathfindingAlgorithms()
        {
            var result = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                            .Where(x => typeof(IPathfindingAlgorithm).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                            .ToList();

            return result;
        }

        /// <summary>
        /// Returns a list of types of all classes which implement INavTilePathModifier.
        /// </summary>
        public List<Type> GetSmoothingAlgorithms()
        {
            var result = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                            .Where(x => typeof(INavTilePathModifier).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                            .ToList();

            return result;
        }

        /// <summary>
        /// Set the currently used algorithm to the type given.
        /// </summary>
        /// <typeparam name="T">Type of the algorithm class. Has to implement IPathfindingAlgorithm.</typeparam>
        public void SetAlgorithm<T>() where T : IPathfindingAlgorithm
        {
            AlgorithmType = typeof(T).AssemblyQualifiedName;
            _algorithm = null;
        }
    }
}
