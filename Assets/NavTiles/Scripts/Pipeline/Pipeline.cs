using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace Snowcap.Pipeline
{
    /// <summary>
    /// Extension method. The first parameter in the function determines the datatype the extension method is available for.
    /// </summary>
    public static class PipelineStepExtensions
    {

        /// <summary>
        /// Extension method to add a step to the pipeline with generic input and output types, which is part of a pipeline with a generic input and output type.
        /// </summary>
        /// <param name="inInputType">The input of the previous step.</param>
        /// <param name="pipelineBuilder">The overall pipeline instance.</param>
        /// <param name="step">Step function which is added to the pipeline.</param>
        /// <param name="inDegreeOfParallism">How many threads this step should use.</param>
        /// <typeparam name="TInput">Input type of the step function.</typeparam>
        /// <typeparam name="TOutput">Output type of the step function.</typeparam>
        /// <typeparam name="TInputOuter">Input type of the overall pipeline.</typeparam>
        /// <typeparam name="TOutputOuter">Output type of the overall pipeline.</typeparam>
        /// <returns>A default value of the Output type of the step function, to be used to chain another step function. The output type can then be used as the input of the following step function.</returns>
        public static TOutput Step<TInput, TOutput, TInputOuter, TOutputOuter>(this TInput inInputType, 
                                                                                    Pipeline<TInputOuter, TOutputOuter> inPipelineBuilder, 
                                                                                    Func<TInput, TOutput> inStep, 
                                                                                    int inDegreeOfParallism)
        {
            var pipelineStep = inPipelineBuilder.GenerateStep<TInput, TOutput>(inDegreeOfParallism);
            pipelineStep.StepAction = inStep;
            return default(TOutput);
        }

        public static TOptionalInOut OptionalStep<TOptionalInOut, TInputOuter, TOutputOuter>(this TOptionalInOut inInputType, 
                                                                                                  Pipeline<TInputOuter, TOutputOuter> inPipelineBuilder, 
                                                                                                  Func<TOptionalInOut, bool> inChoice, Func<TOptionalInOut, TOptionalInOut> inStep, int 
                                                                                                  inDegreeOfParallism)
        {
            var pipelineStep = inPipelineBuilder.GenerateOptionalStep<TOptionalInOut>(inDegreeOfParallism);
            pipelineStep.StepAction = inStep;
            pipelineStep.Choice = inChoice;

            return default(TOptionalInOut);
        }
    }

    /// <summary>
    /// Pipeline class.
    /// </summary>
    /// <typeparam name="TPipeIn">Input type of the pipeline.</typeparam>
    /// <typeparam name="TPipeOut">Output type of the pipeline.</typeparam>
    public class Pipeline<TPipeIn, TPipeOut>
    {
        #region Internal classes

        /// <summary>
        /// Interafce for a pipeline step buffer.
        /// </summary>
        /// <typeparam name="TStepIn">Input to be stored in the buffer.</typeparam>
        public interface IPipelineStep<TStepIn>
        {
            BlockingCollection<Item<TStepIn>> Buffer { get; set; }
        }

        /// <summary>
        /// Pipeline step with an action, which produces an output.
        /// </summary>
        /// <typeparam name="TStepIn">Type of input.</typeparam>
        /// <typeparam name="TStepOut">Type of output.</typeparam>
        public class PipelineStep<TStepIn, TStepOut> : IPipelineStep<TStepIn>
        {
            public BlockingCollection<Item<TStepIn>> Buffer { get; set; } = new BlockingCollection<Item<TStepIn>>();
            public Func<TStepIn, TStepOut> StepAction { get; set; }
        }

        public class OptionalPipelineStep<TStepIn, TStepOut> : PipelineStep<TStepIn, TStepOut> where TStepIn : TStepOut
        {
            public Func<TStepIn, bool> Choice { get; set; }
        }

        /// <summary>
        /// Represents one input for a step, with a TaskCompletionSource which stores the result of a Task which is running on a thread.
        /// </summary>
        /// <typeparam name="TStepIn">Input Type.</typeparam>
        public class Item<TStepIn>
        {
            public TStepIn Input { get; set; }
            public TaskCompletionSource<TPipeOut> TaskCompletionSource { get; set; }
        }

        #endregion // Internal classes.

        // List of steps in the pipeline.
        [SerializeField]
        private List<object> _pipelineSteps = new List<object>();
        public List<object> PipelineSteps { get { return _pipelineSteps; } }

        // Callback which is called with output result.
        public event Action<object> Finished;

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public Pipeline() { }

        /// <summary>
        /// Constructor for the Pipeline. 
        /// 
        /// A pipeline is initialized as follows:
        /// 
        ///  var pipeline = new Pipeline<string, bool>((inputFirst, builder) => 
        ///     inputFirst.Step(builder, input => input.Length, degreeOfParallelism)
        ///     .Step(builder, input => input % 2 == 1, degreeOfParallelism));
        /// 
        /// This pipeline has two steps which check the length of a string and then checks whether this length is uneven.
        /// 
        /// These steps are defined in a anonymous function which is invoked in the constructor.
        /// This triggers the extension methods and generates the steps with the desired threads.
        /// </summary>
        /// <param name="inSteps"></param>
        public Pipeline(Func<TPipeIn, Pipeline<TPipeIn, TPipeOut>, TPipeOut> inSteps)
        {
            inSteps.Invoke(default(TPipeIn), this);
        }

        /// <summary>
        /// Add a step to the pipeline.
        /// </summary>
        /// <param name="inStep">Function to be activated in this step.</param>
        /// <param name="inDegreeOfParallism">How many threads this step should have available.</param>
        /// <typeparam name="TStepIn">Input type of the step.</typeparam>
        /// <typeparam name="TStepOut">Output type of the step.</typeparam>
        /// <returns></returns>
        public void AddStep<TStepIn, TStepOut>(Func<TStepIn, TStepOut> inStep, int inDegreeOfParallism)
        {
            // Check if this connects to the start of the pipeline or a previous step already added.
            // Throws an exception if this is not the case.
            TypeCheckStep(typeof(TStepIn));

            var pipelineStep = GenerateStep<TStepIn, TStepOut>(inDegreeOfParallism);
            pipelineStep.StepAction = inStep;
        }

        /// <summary>
        /// Add an optional step to the pipeline.
        /// </summary>
        /// <param name="inChoice">Function to check whether to active this step.</param>
        /// <param name="inStep">Function to be activated in this step.</param>
        /// <param name="inDegreeOfParallism">How many threads this step should have available.</param>
        /// <typeparam name="TOptionalInOut">In and Output of this step. Has to be the same since the next step doesn't know whether this step will run or not.</typeparam>
        public void AddOptionalStep<TOptionalInOut>(Func<TOptionalInOut, bool> inChoice, Func<TOptionalInOut, TOptionalInOut> inStep, int inDegreeOfParallism)
        {
            // Check if this connects to the start of the pipeline or a previous step already added.
            // Throws an exception if this is not the case.
            TypeCheckStep(typeof(TOptionalInOut));

            var pipelineStep = GenerateOptionalStep<TOptionalInOut>(inDegreeOfParallism);
            pipelineStep.StepAction = inStep;
            pipelineStep.Choice = inChoice;
        }

        /// <summary>
        /// Check the steps whether they properly connect to the previous step.
        /// </summary>
        /// <param name="inStepInputType">The input type of the step being checked.</param>
        private void TypeCheckStep(Type inStepInputType)
        {
            if (PipelineSteps.Count == 0)
            {
                if (inStepInputType != typeof(TPipeIn))
                {
                    throw new Exception("First step does not have the required pipeline input type\nPipeline requires a " + typeof(TPipeIn).Name + ", while the first step requires a " + inStepInputType.Name + " type");
                }
            }
            else
            {
                object last = PipelineSteps[PipelineSteps.Count - 1];
                Type t = last.GetType();

                if (t.IsGenericType)
                {
                    Type returnType = t.GenericTypeArguments.Last();

                    if (returnType != inStepInputType)
                    {
                        throw new Exception("Step does not have the correct input type.\nPrevious step returns a " + returnType.Name + ", while the new step requires a " + inStepInputType.Name + " type");
                    }
                }
                else
                {
                    throw new Exception("Somehow a non-generic type has been added to the pipelinesteps");
                }
            }
        }

        /// <summary>
        /// Executes the pipeline with an initial input.
        /// </summary>
        /// <param name="inInput">The input value.</param>
        /// <returns>Returns the task which being run and holds the result of the pipeline.</returns>
        public Task<TPipeOut> Execute(TPipeIn inInput)
        {
            // Before execute starts, check if the last step outputs the same type as the pipeline should output.
            object last = PipelineSteps[PipelineSteps.Count - 1];
            Type t = last.GetType();

            if (t.IsGenericType)
            {
                Type returnType = t.GenericTypeArguments.Last();

                if (returnType != typeof(TPipeOut))
                {
                    throw new Exception("Last step in the pipeline does not have the correct output type\nThe last step outputs a " + returnType.Name + ", while the pipeline should output a " + typeof(TPipeOut).Name + " type");
                }
            }

            IPipelineStep<TPipeIn> first = PipelineSteps[0] as IPipelineStep<TPipeIn>;
            TaskCompletionSource<TPipeOut> task = new TaskCompletionSource<TPipeOut>();
            first.Buffer.Add(new Item<TPipeIn>()
            {
                Input = inInput,
                TaskCompletionSource = task
            });
            return task.Task;
        }

        /// <summary>
        /// Generates a step in the pipeline with the given number of threads.
        /// </summary>
        /// <param name="inDegreeOfParallism">How many threads should run parallel for this step.</param>
        /// <typeparam name="TStepIn">Input type of the step.</typeparam>
        /// <typeparam name="TStepOut">Output type of the step.</typeparam>
        public PipelineStep<TStepIn, TStepOut> GenerateStep<TStepIn, TStepOut>(int inDegreeOfParallism)
        {
            var pipelineStep = new PipelineStep<TStepIn, TStepOut>();
            var stepIndex = PipelineSteps.Count;

            for (int i = 0; i < inDegreeOfParallism; i++)
            {
                Task.Run(() =>
                {
                    StartStep<TStepIn, TStepOut>(stepIndex, pipelineStep);
                });
            }

            PipelineSteps.Add(pipelineStep);
            return pipelineStep;
        }

        /// <summary>
        /// Processes the step.
        /// </summary>
        /// <param name="inStepIndex">Which step is being processed in the pipeline.</param>
        /// <param name="inPipelineStep">The pipeline step instance.</param>
        private void StartStep<TStepIn, TStepOut>(int inStepIndex, PipelineStep<TStepIn, TStepOut> inPipelineStep)
        {
            IPipelineStep<TStepOut> nextPipelineStep = null;

            // Iterate over all inputs in the buffer for this step
            foreach (var input in inPipelineStep.Buffer.GetConsumingEnumerable())
            {
                TStepOut output;

                // Try to perform the step.
                try
                {
                    output = inPipelineStep.StepAction(input.Input);
                }
                catch (Exception e)
                {
                    input.TaskCompletionSource.SetException(e);
                    continue;
                }

                FinishStep(input, output, inStepIndex, ref nextPipelineStep);
            }
        }

        /// <summary>
        /// Generates an optional step with the given number of threads.
        /// </summary>
        /// <param name="inDegreeOfParallism">How many threads should be available for this step.</param>
        /// <typeparam name="TOptionalInOut">The In- and Output type of this step.</typeparam>
        public OptionalPipelineStep<TOptionalInOut, TOptionalInOut> GenerateOptionalStep<TOptionalInOut>(int inDegreeOfParallism)
        {
            var pipelineStep = new OptionalPipelineStep<TOptionalInOut, TOptionalInOut>();
            var stepIndex = PipelineSteps.Count;

            for (int i = 0; i < inDegreeOfParallism; i++)
            {
                Task.Run(() =>
                {
                    StartOptionalStep<TOptionalInOut>(stepIndex, pipelineStep);
                });
            }

            PipelineSteps.Add(pipelineStep);
            return pipelineStep;
        }

        /// <summary>
        /// Processes the optional step.
        /// </summary>
        /// <param name="inStepIndex">The index of this step in the pipeline.</param>
        /// <param name="inOptionalStep">The optional step being processed.</param>
        /// <typeparam name="TOptionalInOut">The In and Output type of the optional step.</typeparam>
        private void StartOptionalStep<TOptionalInOut>(int inStepIndex, OptionalPipelineStep<TOptionalInOut, TOptionalInOut> inOptionalStep)
        {
            IPipelineStep<TOptionalInOut> nextPipelineStep = null;

            foreach (var input in inOptionalStep.Buffer.GetConsumingEnumerable())
            {
                TOptionalInOut output = default(TOptionalInOut);

                try
                {
                    if (inOptionalStep.Choice(input.Input))
                    {
                        output = inOptionalStep.StepAction(input.Input);
                    }
                }
                catch (Exception e)
                {
                    input.TaskCompletionSource.SetException(e);
                    continue;
                }

                FinishStep(input, output, inStepIndex, ref nextPipelineStep);
            }
        }

        /// <summary>
        /// Completes the step being taken and moves on to the next one or returns the result of the pipeline.
        /// </summary>
        /// <param name="inStepInput">The Input value of the step.</param>
        /// <param name="inStepOutput">The Output value of the step.</param>
        /// <param name="inStepIndex">The index of the step in the pipeline.</param>
        /// <param name="refNextPipelineStep">The next pipeline step to be returned to the caller.</param>
        /// <typeparam name="TStepIn">The Input type of the step.</typeparam>
        /// <typeparam name="TStepOut">The Output type of the step.</typeparam>
        /// <returns></returns>
        private void FinishStep<TStepIn, TStepOut>(Item<TStepIn> inStepInput, TStepOut inStepOutput, int inStepIndex, ref IPipelineStep<TStepOut> refNextPipelineStep)
        {
            bool isLastStep = inStepIndex == PipelineSteps.Count - 1;

            // Incase it is the last step, set the result of the task and invoke the mFinished delegate.
            if (isLastStep)
            {
                inStepInput.TaskCompletionSource.SetResult((TPipeOut)(object)inStepOutput);
                Finished?.Invoke(inStepOutput);
            }
            else // Otherwise, get the next step in the pipeline.
            {
                refNextPipelineStep = refNextPipelineStep ?? PipelineSteps[inStepIndex + 1] as IPipelineStep<TStepOut>;
                refNextPipelineStep.Buffer.Add(new Item<TStepOut>()
                {
                    Input = inStepOutput,
                    TaskCompletionSource = inStepInput.TaskCompletionSource
                });
            }
        }
    }
}
