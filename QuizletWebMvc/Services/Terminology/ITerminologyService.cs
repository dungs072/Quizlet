﻿using QuizletWebMvc.ViewModels.Terminology;
using QuizletWebMvc.ViewModels.User;

namespace QuizletWebMvc.Services.Terminology
{
    public interface ITerminologyService
    {
        Task<List<TitleViewModel>> GetTitlesBaseOnUserId(int UserId);
        Task<List<TitleViewModel>> GetTitles();
        Task CreateTitle(TitleViewModel titleViewModel);
        Task<bool> HasDuplicateTitlePerUser(int userId, string titleName);
        Task<bool> HasDuplicateTitlePerUserForUpdate(int titleId, int userId, string titleName);
        Task<TitleViewModel> GetTitleViewModel(int titleId);
        Task DeleteTitle(int TitleId);
        Task UpdateTitle(TitleViewModel titleViewModel);

        Task<List<LearningModuleViewModel>> GetLearningModuleByTitleId(int TitleId);
        Task<bool> CreateLearningModule(LearningModuleViewModel learningModuleViewModel);
        Task<bool> DeleteLearningModule(int LearningModuleId);
        Task<LearningModuleViewModel> GetLearningModuleViewModel(int learningModuleId);
        Task<bool> UpdateLearningModule(LearningModuleViewModel learningModuleViewModel);

        Task<List<TermViewModel>> GetTermByLearningModuleId(int learningModuleId);
        Task<bool> CreateTerm(TermViewModel termViewModel);
        Task<bool> DeleteTerm(int termId);
        Task<bool> UpdateTerm(TermViewModel termViewModel);
        Task<TermViewModel> GetTermViewModel(int termId);
        Task<List<ObjectivePack>> GetObjectivePacks(int learningModuleId);

    }
}