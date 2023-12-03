﻿using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using System;

namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermController : ControllerBase
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";
        private readonly TerminologyDBContext dbContext;
        public TermController(TerminologyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("{learningModuleId}")]
        public IEnumerable<THETHUATNGU> GetTHETHUATNGUByTitleId(int learningModuleId)
        {
            var thuatngu = dbContext.thethuatngus.Where(e => e.hocphan.LearningModuleId == learningModuleId).ToList();
            return thuatngu;
        }
        [HttpGet("find/{termId}")]
        public async Task<ActionResult<THETHUATNGU>> GetTHUATNGUByLearningModuleId(int termId)
        {
            var thuatngu = await dbContext.thethuatngus.FindAsync(termId);

            return thuatngu;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTHUATNGU(THETHUATNGU thuatngu)
        {
            if (HasDuplicatedTermNamePerLearningModule(thuatngu.hocphan.LearningModuleId, thuatngu.TermName))
            {
                return BadRequest();
            }
            await dbContext.thethuatngus.AddAsync(thuatngu);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicatedTermNamePerLearningModule(int learningModuleId, string termName)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.hocphan.LearningModuleId == learningModuleId && u.TermName == termName));
            return thuatngu != null;
        }
        [HttpDelete("{termId}")]
        public async Task<ActionResult> DeleteTHUATNGU(int termId)
        {
            var THUATNGU = await dbContext.thethuatngus.FindAsync(termId);
            if (THUATNGU.Image != null)
            {
                var cancellation = new CancellationTokenSource();
                // Initialize Firebase Storage
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                });
                string fileNameDelete = ExtractFileNameFromUrl(THUATNGU.Image);
                string deletePath = $"images/{fileNameDelete}";
                await firebaseStorage.Child(deletePath).DeleteAsync();
            }
            dbContext.thethuatngus.Remove(THUATNGU);
            await dbContext.SaveChangesAsync();
            return Ok();

        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTHUATNGU(THETHUATNGU thuatngu)
        {
            if (HasDuplicateTermNamePerLearningModuleForUpdate(thuatngu.TermId, thuatngu.hocphan.LearningModuleId, thuatngu.TermName))
            {
                return BadRequest();
            }

            dbContext.thethuatngus.Update(thuatngu);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateTermNamePerLearningModuleForUpdate(int termId, int learningModuleId, string termName)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.hocphan.LearningModuleId == learningModuleId && u.TermName == termName && u.TermId != termId));
            return thuatngu != null;
        }
        private void Shuffle<T>(List<T> list)
        {
            Random random = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                // Generate a random index between 0 and i (inclusive).
                int randomIndex = random.Next(0, i + 1);

                // Swap elements at randomIndex and i.
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        [HttpGet("objective/{learningModuleId}")]
        public IEnumerable<ObjectivePack> GetObjectiveList(int learningModuleId)
        {
            var thuatngus = dbContext.thethuatngus.Where(e => e.hocphan.LearningModuleId == learningModuleId).ToList();
            
            List<ObjectivePack> objectivePacks = new List<ObjectivePack>();
            Random random = new Random();
            for (int i =0;i<thuatngus.Count;i++)
            {
                ObjectivePack objectivePack = new ObjectivePack();
                objectivePacks.Add(objectivePack);
                objectivePack.Question = thuatngus[i].TermName;
                objectivePack.TermId = thuatngus[i].TermId;
                int randomNumber = -1;
                List<int> exclusions = new List<int>();
                if (thuatngus.Count>4)
                {
                    
                    exclusions.Add(i);
                    for (int j = 0; j < 3; j++)
                    {
                        do
                        {
                            randomNumber = random.Next(0, thuatngus.Count);
                        } while (exclusions.Contains(randomNumber));
                        exclusions.Add(randomNumber);
                    }
                    for(int k =0;k<4;k++)
                    {
                        int index = exclusions[random.Next(0, exclusions.Count)];
                        exclusions.Remove(index);
                        if(k==0)
                        {
                            if(index==i)
                            {
                                objectivePack.Answer = "A";
                            }
                            objectivePack.ChoiceA = thuatngus[index].Explaination;
                        }
                        if (k == 1)
                        {
                            if (index == i)
                            {
                                objectivePack.Answer = "B";
                            }
                            objectivePack.ChoiceB = thuatngus[index].Explaination;
                        }
                        if (k == 2)
                        {
                            if (index == i)
                            {
                                objectivePack.Answer = "C";
                            }
                            objectivePack.ChoiceC = thuatngus[index].Explaination;
                        }
                        if (k == 3)
                        {
                            if (index == i)
                            {
                                objectivePack.Answer = "D";
                            }
                            objectivePack.ChoiceD = thuatngus[index].Explaination;
                        }
                    }

                }
                else
                {
                    objectivePack.Answer = "D";
                    objectivePack.ChoiceD = thuatngus[i].Explaination;
                    exclusions.Add(i);
                    for(int j=0;j<thuatngus.Count;j++)
                    {
                        if(j==0)
                        {
                            if(!exclusions.Contains(j))
                            {
                                exclusions.Add(j);
                                objectivePack.ChoiceA = thuatngus[j].Explaination;
                            }  
                        }
                        if (j == 1)
                        {
                            if (!exclusions.Contains(j))
                            {
                                exclusions.Add(j);
                                objectivePack.ChoiceB = thuatngus[j].Explaination;
                            }
                        }
                        if (j == 2)
                        {
                            if (!exclusions.Contains(j))
                            {
                                exclusions.Add(j);
                                objectivePack.ChoiceC = thuatngus[j].Explaination;
                            }
                        }
                    }
                    
                }
            }
            Shuffle<ObjectivePack>(objectivePacks);
            return objectivePacks;
        }

        [HttpPut("test")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTHUATNGUTest(ResultQuestion resultQuestion)
        {
            var thuatngu = await dbContext.thethuatngus.FindAsync(resultQuestion.TermId);
            if(thuatngu == null) { return BadRequest(); }
            if(resultQuestion.IsRightAnswer)
            {
                thuatngu.Accumulate++;
            }
            else
            {
                thuatngu.Accumulate = Math.Max(thuatngu.Accumulate - 1, 0);
            }
            foreach(var level in dbContext.levelghinhos)
            {
                if(thuatngu.Accumulate>=level.Condition)
                {
                    thuatngu.LevelId = level.LevelId;
                }
            }
            dbContext.thethuatngus.Update(thuatngu);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        static string ExtractFileNameFromUrl(string url)
        {
            // Use Uri to parse the URL
            Uri uri = new Uri(url);

            // Get the filename from the URL using Path.GetFileName
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }
    }
}
