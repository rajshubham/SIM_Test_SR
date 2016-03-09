using System.ComponentModel;

namespace PRGX.SIMTrax.Domain.Util
{
    public enum ResourceType
    {
        Email = 0,
        Message = 1,
        Constants = 2,
        HelpText = 3
    }

    public enum UserType
    {
        [Description("All")]
        None = 0,
        [Description("Buyer")]
        Buyer = 64,
        [Description("Supplier")]
        Supplier = 65,
        [Description("Auditor")]
        Auditor = 68,
        [Description("Admin Buyer")]
        AdminBuyer = 500,
        [Description("Admin Supplier")]
        AdminSupplier = 501,
        [Description("Admin Auditor")]
        AdminAuditor = 502
    }

    public enum AuditorRoles
    {

        [Description("Assign Auditor Roles")]
        AssignRoles = 16,
        [Description("Edit User/profile")]
        EditUser = 17,
        [Description("Change Password")]
        ChangePassword = 18,
        [Description("Create Auditor")]
        CreateAuditor = 19,
        [Description("Create Buyer Role")]
        CreateBuyerRole = 20,
        [Description("Create Auditor Role")]
        CreateAuditorRole = 21,
        [Description("Delete Auditor")]
        DeleteAuditor = 22,
        [Description("Create Questionnaire Section")]
        CreateQuestionnaireSection = 23,
        [Description("Map section to buyers")]
        MapSectionBuyers = 24,
        [Description("Create Question")]
        CreateQuestion = 25,
        [Description("Publish Questionnaire Section")]
        PublishQuestionnaireSection = 26,
        [Description("Create & Edit Campaign")]
        CreateEditCampaign = 27,
        [Description("Assign Campaign")]
        AssignCampaign = 28,
        [Description("Verify Campaign")]
        VerifyCampaign = 29,
        [Description("Approve Campaign")]
        ApproveCampaign = 30,
        [Description("Release Campaign")]
        ReleaseCampaign = 31,
        [Description("Verify Buyer")]
        VerifyBuyer = 32,
        [Description("Activate Buyer")]
        ActivateBuyer = 33,
        [Description("Change Buyer Access Type")]
        ChangeBuyerAccessType = 34,
        [Description("Assign Default Products")]
        AssignDefaultProduct = 35,
        [Description("Buyer Supplier Mapping")]
        BuyerSupplierMapping = 36,
        [Description("Supplier Assign Product")]
        BuyerSupplierAssignProduct = 37,
        [Description("Create User")]
        CreateUser = 38,
        [Description("Add/Edit Referrer")]
        AddReferrer = 39,
        [Description("Details Check - View only")]
        DetailsCheckView = 40,
        [Description("Details Check")]
        DetailsCheck = 41,
        [Description("Profile Check - View only")]
        ProfileCheckView = 42,
        [Description("Profile Check")]
        ProfileCheck = 43,
        [Description("Sanction Check")]
        SanctionCheck = 44,
        [Description("FIT Check - View only")]
        FITCheckView = 45,
        [Description("FIT Check")]
        FITCheck = 46,
        [Description("HS Check - View only")]
        HSCheckView = 47,
        [Description("HS Check")]
        HSCheck = 48,
        [Description("DS Check - View only")]
        DSCheckView = 49,
        [Description("DS Check")]
        DSCheck = 50,
        [Description("Publish Supplier")]
        PublishSupplier = 51,
        [Description("Create Voucher")]
        CreateVoucher = 52,
        [Description("Create Credit Note")]
        CreateCreditNote = 53,
        [Description("Generate Quotation")]
        GenerateQuotation = 54,
        [Description("Authorise Transaction")]
        AuthoriseTransaction = 55,
        [Description("Users")]
        Users = 56,
        [Description("Questionnaire")]
        Questionnaire = 57,
        [Description("Buyers")]
        Buyers = 58,
        [Description("Suppliers")]
        Suppliers = 59,
        [Description("Finance")]
        Finance = 60

    }

    public enum CompanyStatus
    {
        [Description("Started Registration")]
        Started = 503,
        [Description("Submitted Registration")]
        Submitted = 504,
        [Description("Verified Registration")]
        RegistrationVerified = 505,
        [Description("Verified Profile")]
        ProfileVerified = 506,
        [Description("Sanction Verified")]
        SanctionVerified = 507,
        [Description("Published")]
        Published = 508,
        [Description("Not Registered")]
        NotRegistered = 1
    }
    public enum SortOrder
    {
        Asc = 1,
        Desc = 2,
        Normal = 3
    }
    public enum SupplierType
    {
        [Description("All")]
        None = 0,
        [Description("Favourite suppliers")]
        Favourite = 1,
        [Description("Suppliers you trade with")]
        TradingWith = 2,
        Both = 3
    }


    public enum AddressType
    {
        [Description("Mailing")]
        Primary = 509,
        [Description("Registered")]
        Registered = 510,
        [Description("HeadQuarters")]
        HeadQuarters = 511,
        [Description("Remittance")]
        Remittance = 512,
        [Description("Branch")]
        Branch = 513,
        [Description("Third Party")]
        ThirdParty = 514
    }

    public enum ContactType
    {
        Primary = 515,
        Procurement = 518,
        HS = 519,
        Accounts = 516,
        Sustainability = 517
    }

    public enum ProjectSource
    {
        [Description("BOTH")]
        Both = 0,
        [Description("CIPS")]
        CIPS = 520,
        [Description("SIM")]
        SIM = 521
    }

    public enum BuyerOrganisationStatus
    {
        All = 0,
        SubmittedRegistration = 1,
        VerifiedRegistration = 2,
        VerifiedAndActivated = 3
    }

    public enum RoleType
    {
        [Description("All")]
        None = 0,
        [Description("Buyer Access")]
        BuyerAccess = 4,
        [Description("Auditor Role")]
        AuditorRole = 5
    }

    public enum BuyerPermissions
    {
        [Description("Dashboard – Compliance")]
        DashboardCompliance = 1,
        [Description("Dashboard - Onboarding")]
        DashboardOnboarding = 2,
        [Description(" Search Page")]
        Search = 3,
        [Description("Key Questions")]
        KeyQuestions = 4,
        [Description("Risk Analysis")]
        RiskAnalysis = 5,
        [Description("Supplier Onboarding")]
        SupplierOnboarding = 6,
        [Description("Inbox")]
        Inbox = 7,
        [Description(" Compliance Status")]
        ComplianceStatus = 8,
        [Description(" General Information")]
        GeneralInformation = 9,
        [Description("Contacts")]
        Contacts = 10,
        [Description("Addresses")]
        Addresses = 11,
        [Description("Bank Details")]
        BankDetails = 12,
        [Description("References")]
        References = 13,
        [Description("Filters & Results - Answers")]
        FiltersResults = 14,
        [Description("Registration Checks")]
        RegistrationChecks = 15
    }


    public enum ComplianceScoreCategory
    {
        [Description("All")]
        All = 0,
        [Description("Profile")]
        Profile = 1,
        [Description("Finance, Insurance & Tax")]
        FinanceInsuranceTax = 2,
        [Description("Health & Safety")]
        HealthSafety = 3,
        [Description("Certificates & Licenses")]
        CertificatesLicenses = 4,
        [Description("Data Security")]
        DataSecurity = 5
    }

    public enum CampaignType
    {
        [Description("Landing Page")]
        NotRegistered = 522,
        [Description("Pre Registered")]
        PreRegistered = 523,
        [Description("Public Data")]
        PublicData = 524
    }

    public enum CampaignStatus
    {
        [Description("Awaiting Assignment")]
        None = 525,
        [Description("Assigned")]
        Assigned = 526,
        [Description("Submitted")]
        Submitted = 527,
        [Description("Approved")]
        Approved = 528,
        [Description("Released")]
        Release = 529
    }

    public enum CampaignLandingTemplate
    {
        [Description("PRGX Template")]
        PrgxTemplate = 530,
        [Description("Client Branded Template")]
        ClientTemplate = 531,
    }

    public enum VerifySupplierReportStatus
    {
        [Description("Awaiting")]
        Awaiting = 0,
        [Description("In Progress")]
        InProgess = 1,
        [Description("Verified")]
        Verifed = 2
    }
}
