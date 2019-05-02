using System;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Services {
    public interface IOCRDataExtractor {
        Task<UserExpense> ExtractExpensesFromReceipt(RecognitionResult recognitionResult, Metadata imageMetadata);
        Task<UserExpense> ExtractExpensesFromTypeOneBill(RecognitionResult recognitionResult, Metadata imageMetadata);
        Task<UserExpense> ExtractExpensesFromTypeTwoBill(RecognitionResult recognitionResult, Metadata imageMetadata);
    }
}
