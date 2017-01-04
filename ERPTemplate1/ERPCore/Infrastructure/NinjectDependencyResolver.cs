using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using ERPDomain.Abstract;
using ERPDomain.Concrete;

namespace ERPCore.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IParentMenuRepository>().To<EFParentMenuRepository>();
            kernel.Bind<ISubMenuRepository>().To<EFSubMenuRepository>();
            kernel.Bind<ISH_USERRepository>().To<EFSH_USERRepository>();
            kernel.Bind<ISH_ROLERepository>().To<EFSH_ROLERepository>();
            kernel.Bind<ISH_APPRepository>().To<EFSH_APPRepository>();
            kernel.Bind<ISH_ROLEMEMBERRepository>().To<EFSH_ROLEMEMBERRepository>();
            kernel.Bind<ISH_ROLEACCESSRepository>().To<EFSH_ROLEACCESSRepository>();
            kernel.Bind<IPluginAreaRepository>().To<EFPluginAreaRepository>();
            kernel.Bind<IDynamicMenuRepository>().To<EFDynamicMenuRepository>();

            kernel.Bind<IAccountGroupRepository>().To<EFAccountGroupRepository>();
            kernel.Bind<IActivityGroupRepository>().To<EFActivityGroupRepository>();
            kernel.Bind<IChartOfAccountRepository>().To<EFChartOfAccountRepository>();
            kernel.Bind<IActivityRepository>().To<EFActivityRepository>();
            kernel.Bind<IActivitySubRepository>().To<EFActivitySubRepository>();
            kernel.Bind<IJournalRepository>().To<EFJournalRepository>();
            kernel.Bind<IJournalLineRepository>().To<EFJournalLineRepository>();

            kernel.Bind<IInvoiceRepository>().To<EFInvoiceRepository>();
            kernel.Bind<IInvoiceLineRepository>().To<EFInvoiceLineRepository>();
            kernel.Bind<IPaymentRepository>().To<EFPaymentRepository>();
            kernel.Bind<IPaymentLineRepository>().To<EFPaymentLineRepository>();
            kernel.Bind<IVendorRepository>().To<EFVendorRepository>();
            kernel.Bind<ICustomerRepository>().To<EFCustomerRepository>();
            kernel.Bind<IChartOfAccount_DimensionValueRepository>().To<EFChartOfAccount_DimensionValueRepository>();
            kernel.Bind<IDimensionSetupRepository>().To<EFDimensionSetupRepository>();
            kernel.Bind<IDimension_TableRelationshipRepository>().To<EFDimension_TableRelationshipRepository>();
            kernel.Bind<IDimension_SettingRepository>().To<EFDimension_SettingRepository>();
            kernel.Bind<IDimension_ValueRepository>().To<EFDimension_ValueRepository>();
            kernel.Bind<IChartOfAccount_Dim_SetupRepository>().To<EFChartOfAccount_Dim_SetupRepository>();
            kernel.Bind<IChartOfAccount_Dim_ValueRepository>().To<EFChartOfAccount_Dim_ValueRepository>();
            kernel.Bind<ICustomer_Dim_SetupRepository>().To<EFCustomer_Dim_SetupRepository>();
            kernel.Bind<ICustomer_Dim_ValueRepository>().To<EFCustomer_Dim_ValueRepository>();
            kernel.Bind<ICreditNoteRepository>().To<EFCreditNoteRepository>();
            kernel.Bind<ICreditNoteLineRepository>().To<EFCreditNoteLineRepository>();
            kernel.Bind<IDebitNoteRepository>().To<EFDebitNoteRepository>();
            kernel.Bind<IDebitNoteLineRepository>().To<EFDebitNoteLineRepository>();

            kernel.Bind<IErrorLogRepository>().To<EFErrorLogRepository>();
            kernel.Bind<IDebugLogRepository>().To<EFDebugLogRepository>();
            kernel.Bind<ISH_USERROLERepository>().To<EFSH_USERROLERepository>();
            kernel.Bind<ILanguageRepository>().To<EFLanguageRepository>();
            kernel.Bind<ICurrencyRepository>().To<EFCurrencyRepository>();
            kernel.Bind<ICurrencyExchangeRateRepository>().To<EFCurrencyExchangeRateRepository>();
            kernel.Bind<ICalendarRepository>().To<EFCalendarRepository>();
            kernel.Bind<ICountryRepository>().To<EFCountryRepository>();
            kernel.Bind<ICountryStateRepository>().To<EFCountryStateRepository>();
            kernel.Bind<IOrganizationRepository>().To<EFOrganizationRepository>();

            kernel.Bind<IUserPreferenceRepository>().To<EFUserPreferenceRepository>();
            kernel.Bind<IMonthEndTransactionRepository>().To<EFMonthEndTransactionRepository>();
            kernel.Bind<IJobScheduleRepository>().To<EFJobScheduleRepository>();

            //Workflow
            kernel.Bind<IWfProcessRepository>().To<EFWfProcessRepository>();
            kernel.Bind<IWfProcessAdminRepository>().To<EFWfProcessAdminRepository>();
            kernel.Bind<IWfGroupRepository>().To<EFWfGroupRepository>();
            kernel.Bind<IWfGroupMemberRepository>().To<EFWfGroupMemberRepository>();
            kernel.Bind<IWfEscalationRepository>().To<EFWfEscalationRepository>();
            kernel.Bind<IWfRequestRepository>().To<EFWfRequestRepository>();
            kernel.Bind<IWfRequestDataRepository>().To<EFWfRequestDataRepository>();
            kernel.Bind<IWfRequestNoteRepository>().To<EFWfRequestNoteRepository>();
            kernel.Bind<IWfRequestFileRepository>().To<EFWfRequestFileRepository>();
            kernel.Bind<IWfRequestStakeholderRepository>().To<EFWfRequestStakeholderRepository>();
            kernel.Bind<IWfRequestActionRepository>().To<EFWfRequestActionRepository>();
            kernel.Bind<IWfStateTypeRepository>().To<EFWfStateTypeRepository>();
            kernel.Bind<IWfStateRepository>().To<EFWfStateRepository>();
            kernel.Bind<IWfStateActivityRepository>().To<EFWfStateActivityRepository>();
            kernel.Bind<IWfTransitionRepository>().To<EFWfTransitionRepository>();
            kernel.Bind<IWfTransitionActionRepository>().To<EFWfTransitionActionRepository>();
            kernel.Bind<IWfTransitionActivityRepository>().To<EFWfTransitionActivityRepository>();
            kernel.Bind<IWfActionTypeRepository>().To<EFWfActionTypeRepository>();
            kernel.Bind<IWfActionRepository>().To<EFWfActionRepository>();
            kernel.Bind<IWfActionTargetRepository>().To<EFWfActionTargetRepository>();
            kernel.Bind<IWfActivityTypeRepository>().To<EFWfActivityTypeRepository>();
            kernel.Bind<IWfActivityRepository>().To<EFWfActivityRepository>();
            kernel.Bind<IWfActivityTargetRepository>().To<EFWfActivityTargetRepository>();
            kernel.Bind<IWfTargetRepository>().To<EFWfTargetRepository>();
            kernel.Bind<IDfMasterRepository>().To<EFDfMasterRepository>();
            kernel.Bind<IDfMasterDataRepository>().To<EFDfMasterDataRepository>();
            kernel.Bind<IDfRequestRepository>().To<EFDfRequestRepository>();
            kernel.Bind<IDfRequestDataRepository>().To<EFDfRequestDataRepository>();
            kernel.Bind<IDfExemptItemRepository>().To<EFDfExemptItemRepository>();
            kernel.Bind<IDfAppliedModuleRepository>().To<EFDfAppliedModuleRepository>();
            kernel.Bind<IEmployeeRepository>().To<EFEmployeeRepository>();
            kernel.Bind<IDfItemTypeRepository>().To<EFDfItemTypeRepository>();
        }
    }
}