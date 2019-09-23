using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadSpeeder
{
    public class ThreadHelper<TList, TObject, TListReturn, TObjectReturn> where TList : List<TObject> where TObject : class where TListReturn : List<TObjectReturn>
    {

        public void PreProcess(TList objectList, TListReturn listRetorno, short simultaneousThreadNumber, Func<TObject, TObjectReturn> methodToBeCalled)
        {
            if (objectList.Count > 0)
            {
                int objectListCounter = 0;
                int indexProcessed = 0;

                for (int i = 0; i < objectList.Count; i++)
                {
                    objectListCounter = objectListCounter + 1;
                    if (objectListCounter == simultaneousThreadNumber)
                    {
                        TList listOfObjectToBeProcessedByRange = (TList)objectList.GetRange(indexProcessed, simultaneousThreadNumber);
                        Processar(listOfObjectToBeProcessedByRange, listRetorno, methodToBeCalled);
                        objectListCounter = 0;
                        indexProcessed += simultaneousThreadNumber;
                    }
                }
                if (objectListCounter > 0)
                {
                    var lastestObjectsToBeProcessed = (TList)objectList.GetRange(objectList.Count - objectListCounter, objectList.Count - indexProcessed);
                    Processar(lastestObjectsToBeProcessed, listRetorno, methodToBeCalled);
                }
            }
        }
        private TListReturn Processar(TList objectList, TListReturn listRetorno, Func<TObject, TObjectReturn> methodToBeCalled)
        {
            try
            {
                using (var countdownEvent = new CountdownEvent(objectList.Count))
                {
                    foreach (var objeto in objectList)
                    {
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            try
                            {
                                //Faz o processamento
                                listRetorno.Add(methodToBeCalled(objeto));
                            }
                            finally
                            {
                                countdownEvent.Signal();
                            }
                        },
                        objectList.IndexOf(objeto)
                        );
                    }
                    countdownEvent.Wait();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listRetorno;
        }
    }
}
