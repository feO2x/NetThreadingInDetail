using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable RedundantAssignment
#pragma warning disable 169

namespace AsyncAwaitDecompiled
{
    internal class AsyncDecompiled
    {
        [DebuggerStepThrough]
        [AsyncStateMachine(typeof(DemoStateMachine))]
        public static Task<int> CalculateTotalNumberOfWordsAsync(IEnumerable<TextReader> readers) // Starting point
        {
            var machine = new DemoStateMachine
            {
                readers = readers, // Copy all method parameter values to the state machine
                Builder = AsyncTaskMethodBuilder<int>.Create(), // Create an AsyncTaskMethodBuilder
                State = -1 // Initial state
            };
            machine.Builder.Start(ref machine); // Start the state machine via the builder, this call might complete the task
            return machine.Builder.Task; // Return the associated task
        }

        [CompilerGenerated]
        private struct DemoStateMachine : IAsyncStateMachine
        {
            // Parameters and Variables
            public IEnumerable<TextReader> readers;
            public int totalNumberOfWords;
            public IEnumerator<TextReader> Enumerator;
            public TextReader reader;
            public string content;

            // Async Infrastructure
            public AsyncTaskMethodBuilder<int> Builder;
            public int State; // -2 = done (successful or exception caught), -1 = running, other states for different await statements
            public TaskAwaiter<string> TaskAwaiter;
            public object Stack; // for expressions like (await a) + (await b)

            void IAsyncStateMachine.MoveNext()
            {
                var result = default(int);
                try
                {
                    var executeFinallyBlock = true;
                    switch (State)
                    {
                        case 0:
                            goto FakeAwaitContinuation;
                    }

                    // Initially, State is -1
                    totalNumberOfWords = 0;
                    Enumerator = readers.GetEnumerator();
                    FakeAwaitContinuation:
                    try
                    {
                        if (State == 0) goto RealAwaitContinuation;
                        goto LoopCondition;

                        LoopBody:
                        reader = Enumerator.Current;
                        var localTaskAwaiter = reader.ReadToEndAsync().GetAwaiter();
                        if (localTaskAwaiter.IsCompleted)
                            goto AwaitCompletion;

                        State = 0;
                        TaskAwaiter = localTaskAwaiter;
                        Builder.AwaitOnCompleted(ref localTaskAwaiter, ref this);
                        executeFinallyBlock = false;
                        return;

                        RealAwaitContinuation:
                        localTaskAwaiter = TaskAwaiter;
                        TaskAwaiter = default(TaskAwaiter<string>);
                        State = -1;

                        AwaitCompletion:
                        content = localTaskAwaiter.GetResult();
                        localTaskAwaiter = default(TaskAwaiter<string>);
                        totalNumberOfWords += content.CalculateNumberOfWords();
                        result = totalNumberOfWords;

                        LoopCondition:
                        if (Enumerator.MoveNext())
                            goto LoopBody;
                    }
                    finally
                    {
                        if (executeFinallyBlock)
                            Enumerator?.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    State = -2;
                    Builder.SetException(exception);
                    return;
                }
                Done:
                State = -2;
                Builder.SetResult(result);
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine) // stateMachine is the boxed version of the DemoStateMachine instance
            {
                Builder.SetStateMachine(stateMachine);
            }
        }
    }
}