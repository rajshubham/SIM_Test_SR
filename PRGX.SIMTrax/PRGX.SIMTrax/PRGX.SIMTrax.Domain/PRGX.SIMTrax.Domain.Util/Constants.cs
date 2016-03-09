using System;

namespace PRGX.SIMTrax.Domain.Util
{
    public class Constants
    {
        #region Constants
        public const string RESOURCE_EMAIL_CONSTANTS = "PRGX.SIMTrax.Domain.Resource.EmailConstants";
        public const string RESOURCE_DISPLAY_MESSAGE_CONSTANTS = "Resources.DisplayMessages";
        public const string RESOURCE_SIMTRAX_CONSTANTS = "PRGX.SIMTrax.Domain.Resource.SimTraxConstants";
        public const string PROFILE_HELP_TEXT_CONSTANTS = "Resources.ProfileHelpText";
        public const string RESOURCE_INTERNATIONALISATION = "PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource";


        public const string ENVIRONMENT_DEV = "DEV";
        public const string ENVIRONMENT_PROD = "PROD";
        public const string ENVIRONMENT_UAT = "UAT";
        public const string ThemeName = "ThemeName";
        public const string USER_ALREADY_EXISTS = "USER_ALREADY_EXISTS";
        public const string ORGANISATION_ALREADY_EXISTS = "ORGANISATION_ALREADY_EXISTS";
        public const string DUPLICATE_PRE_REG_SUPPLIER_RECORD = "DUPLICATE_PRE_REG_SUPPLIER_RECORD";
        public const string SUPPLIER_NAME_REQUIRED = "SUPPLIER_NAME_REQUIRED";
        public const string LOGIN_ID_REQUIRED = "LOGIN_ID_REQUIRED";
        public const string INVALID_EMAIL_ID = "INVALID_EMAIL_ID";
        public const string SUPPLIER_REGISTERED_BY_OTHER_CAMPAIGN = "SUPPLIER_REGISTERED_BY_OTHER_CAMPAIGN";
        public const string INVALID_USER_DETAILS = "INVALID_USER_DETAILS";
        public const string ACCOUNT_LOCKED = "ACCOUNT_LOCKED";
        public const int ACCOUNT_LOCK_COUNT = 2;
        public const string TEMPORARY_USER = "TEMPORARY_USER";
        public const string QUESTIONSET_SAVED_SUCCESSFULLY = "QUESTIONSET_SAVED_SUCCESSFULLY";
        public const string QUESTION_SAVED_SUCCESSFULLY = "QUESTION_SAVED_SUCCESSFULLY";
        public const string SAVED_SUCCESSFULLY = "SAVED_SUCCESSFULLY";
        public const string SUPPLIER_VERIFIED_SUCCESSFULLY = "SUPPLIER_VERIFIED_SUCCESSFULLY";
        public const string SUPPLIER_BUSINESS_VERIFIED_SUCCESSFULLY = "SUPPLIER_BUSINESS_VERIFIED_SUCCESSFULLY";
        public const string BUYER_VERIFIED_SUCCESSFULLY = "BUYER_VERIFIED_SUCCESSFULLY";
        public const string TRADING_SUPPLIER_ADDED_SUCCESSFULLY = "TRADING_SUPPLIER_ADDED_SUCCESSFULLY";
        public const string FAVOURITE_SUPPLIER_ADDED_SUCCESSFULLY = "FAVOURITE_SUPPLIER_ADDED_SUCCESSFULLY";
        public const string TRADING_SUPPLIER_REMOVED_SUCCESSFULLY = "TRADING_SUPPLIER_REMOVED_SUCCESSFULLY";
        public const string FAVOURITE_SUPPLIER_REMOVED_SUCCESSFULLY = "FAVOURITE_SUPPLIER_REMOVED_SUCCESSFULLY";
        public const string SUPPLIER_UNASSIGNED_SUCCESSFULLY = "SUPPLIER_UNASSIGNED_SUCCESSFULLY";
        public const string SUPPLIER_ASSIGNED_SUCCESSFULLY = "SUPPLIER_ASSIGNED_SUCCESSFULLY";
        public const string BUYER_UNASSIGNED_SUCCESSFULLY = "BUYER_UNASSIGNED_SUCCESSFULLY";
        public const string BUYER_ASSIGNED_SUCCESSFULLY = "BUYER_ASSIGNED_SUCCESSFULLY";
        public const string BUYER_ACCESSTYPE_CHANGED_SUCCESSFULLY = "BUYER_ACCESSTYPE_CHANGED_SUCCESSFULLY";
        public const string SUPPLIER_PUBLISHED_SUCCESSFULLY = "SUPPLIER_PUBLISHED_SUCCESSFULLY";
        public const string CAMPAIGN_APPROVED_SUCCESSFULLY = "CAMPAIGN_APPROVED_SUCCESSFULLY";
        public const string UPDATE_MASTER_VENDOR = "UPDATE_MASTER_VENDOR";
        public const string ACCOUNT_CREATED_SUCCESSFULLY = "ACCOUNT_CREATED_SUCCESSFULLY";
        public const string SUBMIT_FOR_VERIFICATION = "SUBMIT_FOR_VERIFICATION";
        public const string INVOICE_ID = "INVOICE_ID";
        public const string BUYER_ACTIVATED_SUCCESSFULLY = "BUYER_ACTIVATED_SUCCESSFULLY";
        public const string LOGIN_ID_ALREADY_EXISTS = "LOGIN_ID_ALREADY_EXISTS";
        public const string ERROR_TO_LOAD = "ERROR_TO_LOAD";
        public const string DELETED_SUCCESSFULLY = "DELETED_SUCCESSFULLY";
        public const string SAVED_SUPPLIER_DETAILS_SUCCESSFULLY = "SAVED_SUPPLIER_DETAILS_SUCCESSFULLY";
        public const string CHANGED_PASSWORD_SUCCESSFULLY = "CHANGED_PASSWORD_SUCCESSFULLY";
        public const string ZERO_QUESTIONS_FOR_QUESTIONSET = "ZERO_QUESTIONS_FOR_QUESTIONSET";
        public const string CAMPAIGN_BUYER_CONTENT = "CAMPAIGN_BUYER_CONTENT";

        #endregion

        #region PartyTypeConstants

        public const string PARTY_TYPE_PERSON = "PERSON";
        public const string PARTY_TYPE_ORGANIZATION = "ORGANIZATION";

        #endregion


        #region ProfileTypeConstants

        public const string PROFILE_TYPE_ADDRESS = "ADDRESS";
        public const string PROFILE_TYPE_BANK = "BANK";

        #endregion

        #region PersonTypeConstants

        public const string PERSON_TYPE_USER = "USER";
        public const string PERSON_TYPE_CONTACT_PERSON = "CONTACT_PERSON";

        # endregion

        #region OrganizationTypeConstants

        public const string ORGANIZATION_TYPE_SELLER = "SELLER";
        public const string ORGANIZATION_TYPE_BUYER = "BUYER";

        # endregion

        #region PartyPartyLinkTypeConstants

        public const string PRIMARY_ORGANIZATION = "PRIMARY_ORGANIZATION";
        public const string CONTACT_ORGANIZATION = "CONTACT_ORGANIZATION";
        public const string CONTACT_BUYER = "CONTACT_BUYER";
        public const string BUYER_TRADING_SUPPLIER = "BUYER_TRADING_SUPPLIER";
        public const string BUYER_FAVOURITE_SUPPLIER = "BUYER_FAVOURITE_SUPPLIER";


        # endregion

        #region PartyIdentifierTypeConstants

        public const string IDENTIFIER_TYPE_VAT_NUMBER = "VAT_NUMBER";
        public const string IDENTIFIER_TYPE_DUNS_NUMBER = "DUNS_NUMBER";
        public const string IDENTIFIER_TYPE_REGISTRATION_NUMBER = "REGISTRATION_NUMBER";
        public const string IDENTIFIER_TYPE_PARENT_DUNS_NUMBER = "PARENT_DUNS_NUMBER";

        # endregion

        #region PartyRegionTypeConstants

        public const string PARTY_REGION_SALES_REGION = "SALES_REGION";
        public const string PARTY_REGION_SERVICE_REGION = "SERVICE_REGION";

        # endregion

        #region ContactMethodConstants

        public const string CONTACT_METHOD_ADDRESS = "ADDRESS";
        public const string CONTACT_METHOD_EMAIL = "EMAIL";
        public const string CONTACT_METHOD_PHONE = "PHONE";

        # endregion

        #region PhoneTypeConstants

        public const string PHONE_TYPE_TELEPHONE = "TELEPHONE";
        public const string PHONE_TYPE_FAX = "FAX";

        #endregion

        #region Document Mnemonics

        public const string BRITVIC_EBP_PDF = "BRITVIC_EBP_PDF";
        public const string BRITVIC_EBP_DOC = "BRITVIC_EBP_DOC";

        #endregion

        #region Role Type

        public const string ROLE_TYPE_AUDITOR = "Auditor";
        public const string ROLE_TYPE_BUYER = "Buyer";

        #endregion

        #region Session Keys
        public const string SESSION_USER = "SESSION_USER";
        public const string SESSION_LOGIN_ID = "LOGIN_ID";
        public const string SESSION_ORGANIZATION = "SESSION_ORGANIZATION";
        public const string SESSION_USER_TYPE = "SESSION_USER_TYPE";
        public const string SESSION_SUBMIT_VERIFICATION = "SESSION_SUBMIT_VERIFICATION";
        //public const string SUPPLIER_FORM_FILE = "SUPPLIER_FORM_FILE";
        //public const string SUPPLIER_LOGO_FILE = "SUPPLIER_LOGO_FILE";
        //public const string SESSION_VERIFY_BUYER_DETAILS = "VerifyBuyerDetails";
        //public const string SESSION_COMPANY_NAME = "SessionCompanyName";
        //public const string SESSION_SUPPLIER_VAT_FORM = "SupplierVATForm";
        //public const string SESSION_SUPPLIER_VAT_FORM_FILENAME = "SupplierVATFormFileNAame";
        //public const string SESSION_SUPPLIER_COMPANY_ID = "SsupplierCompanyId";
        //public const string SESSION_BUYER_COMPANY_ID = "BuyerCompanyId";
        //public const string SESSION_SANCTION_COMPANY_ID = "SessionSanctionCompanyId";
        //public const string SESSION_PROFILE_COMPANY_ID = "SESSION_PROFILE_COMPANY_ID";
        public const string SESSION_CAMPAIGN_ID = "SessionCampaignId";
        public const string SESSION_CAMPAIGN_LOGO = "SessionCampaignLogo";
        public const string SESSION_PRE_REG_ID = "SessionPreRegId";
        public const string SESSION_BUYER_NAME = "BuyerName";
        //public const string SESSION_TRANSACTION_ID = "SESSION_TRANSACTION_ID";
        //public const string SESSION_EVALUATE_PRODUCT = "SESSION_EVALAUTE_PRODUCT";
        //public const string SUPPLIER_PROFILE_ANSWERS = "SUPPLIER_PROFILE_ANSWERS";
        //public const string BUYER_QUESTION_SETS = "BUYER_QUESTION_SETS";
        public const string SESSION_BUYER_ACCESS_PERMISSIONS = "SESSION_BUYER_ACCESS_PERMISSIONS";
        public const string SESSION_AUDITOR_ACCESS_PERMISSIONS = "SESSION_AUDITOR_ACCESS_PERMISSIONS";
        //public const string SESSION_INVOICE_ID = "SESSION_INVOICE_ID";
        #endregion

        #region PlaceHolder Constants
        public static readonly string FIRST_NAME_PLACEHOLDER = "FIRST_NAME_PLACEHOLDER";
        public static readonly string LAST_NAME_PLACEHOLDER = "LAST_NAME_PLACEHOLDER";
        public static readonly string SEARCH_CATEGORY_PLACEHOLDER = "SEARCH_CATEGORY_PLACEHOLDER";
        public const string LOGIN_ID_PLACEHOLDER = "LOGIN_ID_PLACEHOLDER";
        #endregion

        #region Resource File Contants
        public static readonly bool IS_EMAIL_ON = Convert.ToBoolean(ReadResource.GetResource("IS_EMAIL_ON", ResourceType.Email));
        public static readonly string FROM_EMAIL_ID = ReadResource.GetResource("FROM_EMAIL_ID", ResourceType.Email);
        public static readonly string EMAIL_HOST = ReadResource.GetResource("EMAIL_HOST", ResourceType.Email);
        public static readonly string EMAIL_USER_ID = ReadResource.GetResource("EMAIL_USER_ID", ResourceType.Email);
        public static readonly string EMAIL_PASSWORD = ReadResource.GetResource("EMAIL_PASSWORD", ResourceType.Email);
        public static readonly bool EMAIL_SSLON = Convert.ToBoolean(ReadResource.GetResource("EMAIL_SSLON", ResourceType.Email));
        public static readonly bool EMAIL_NEEDS_AUTHENTICATION = Convert.ToBoolean(ReadResource.GetResource("EMAIL_NEEDS_AUTHENTICATION", ResourceType.Email));
        public static readonly int EMAIL_PORT = Convert.ToInt32(ReadResource.GetResource("EMAIL_PORT", ResourceType.Email)); // 587;   
        public static readonly string CAMPAIGN_PRE_REG_SUPPLIER_FOLDER_PATH = ReadResource.GetResource("CAMPAIGN_PRE_REG_SUPPLIER_FOLDER_PATH", ResourceType.Constants);
        public static readonly string BULK_MAPPING_FOLDER_PATH = ReadResource.GetResource("BULK_MAPPING_FOLDER_PATH", ResourceType.Constants);
        public static readonly string CONTACT_EMAIL = ReadResource.GetResource("CONTACT_EMAIL", ResourceType.Constants);
        public static readonly string CONTACT_NUMBER = ReadResource.GetResource("CONTACT_NUMBER", ResourceType.Constants);
        public static readonly string GENERIC_EXCEL_WORKBOOK_PATH = ReadResource.GetResource("GENERIC_EXCEL_WORKBOOK_PATH", ResourceType.Constants);
        public static readonly string ROLE_CREATED = "ROLE_CREATED";
        public static readonly string AUDITOR_CREATED_SUCCESS = "AUDITOR_CREATED_SUCCESS";
        public static readonly string ROLE_DELETED = "ROLE_DELETED";
        public static readonly string UPDATE_MESSAGE = "UPDATE_MESSAGE";
        public static readonly string VALIDATION_MESSAGE = "VALIDATION_MESSAGE";
        public static readonly string DEFAULT_ERROR_MESSAGE = "DEFAULT_ERROR_MESSAGE";
        public static readonly string CAMPAIGN_PRE_REG_SUPPLIER_SAMPLE_FILE_PATH = ReadResource.GetResource("CAMPAIGN_PRE_REG_SUPPLIER_SAMPLE_FILE_PATH", ResourceType.Constants);
        public static readonly string CAMPAIGN_PUBLIC_DATA_SUPPLIER_SAMPLE_FILE_PATH = ReadResource.GetResource("CAMPAIGN_PUBLIC_DATA_SUPPLIER_SAMPLE_FILE_PATH", ResourceType.Constants);
        public static readonly string USER_DELETED = "USER_DELETED";
        public static readonly string DOCUMENT_DELETED = ReadResource.GetResource("DOCUMENT_DELETED", ResourceType.Message);
        public static readonly string ANSWER_SAVED = ReadResource.GetResource("ANSWER_SAVED", ResourceType.Message);
        public static readonly string ANSWERS_SUBMITTED = ReadResource.GetResource("ANSWERS_SUBMITTED", ResourceType.Message);
        public static readonly string ANSWERS_EVALUATED = ReadResource.GetResource("ANSWERS_EVALUATED", ResourceType.Message);
        public static readonly string ANSWERS_REVERTED_TO_NOTSUBMITTED = ReadResource.GetResource("ANSWERS_REVERTED_TO_NOTSUBMITTED", ResourceType.Message);
        public static readonly string QUESTION_SET_CREATED = "QUESTION_SET_CREATED";
        public static readonly string EMAIL_SENT_SUCCESSFULLY = ReadResource.GetResource("EMAIL_SENT_SUCCESSFULLY", ResourceType.Message);
        public static readonly string QUESTIONNIARE_SUBMITT_NOT_VERIFIED = ReadResource.GetResource("QUESTIONNIARE_SUBMITT_NOT_VERIFIED", ResourceType.Message);
        public static readonly string VOUCHER_APPLIED_SUCCESSFULLY = ReadResource.GetResource("VOUCHER_APPLIED_SUCCESSFULLY", ResourceType.Message);
        public static readonly string VOUCHER_APPLIER_ERROR = ReadResource.GetResource("VOUCHER_APPLIED_ERROR", ResourceType.Message);
        public static readonly string SUCCESSFULLY_MAPPED = ReadResource.GetResource("SUCCESSFULLY_MAPPED", ResourceType.Message);
        public static readonly string SUCCESSFULLY_REMOVED_MAPPING = ReadResource.GetResource("SUCCESSFULLY_REMOVED_MAPPING", ResourceType.Message);
        public static readonly string SUCCESS_PRODUCT_MAPPED = ReadResource.GetResource("SUCCESS_PRODUCT_MAPPED", ResourceType.Message);
        public static readonly string SUCCESS_PRODUCT_UNMAPPED = ReadResource.GetResource("SUCCESS_PRODUCT_UNMAPPED", ResourceType.Message);
        public static readonly string SUCCESSFULLY_ADDED_TO_DEFAULT_PRODUCTS = "SUCCESSFULLY_ADDED_TO_DEFAULT_PRODUCTS";
        public static readonly string SUCCESSFULLY_REMOVED_FROM_DEFAULT_PRODUCTS = "SUCCESSFULLY_REMOVED_FROM_DEFAULT_PRODUCTS";
        public static readonly string ALL_SUPPLIER_ARE_UNMAPPED_SUCCESSFULLY = ReadResource.GetResource("ALL_SUPPLIER_ARE_UNMAPPED_SUCCESSFULLY", ResourceType.Message);
        public static readonly string ALL_SUPPLIER_ARE_MAPPED_SUCCESSFULLY = ReadResource.GetResource("ALL_SUPPLIER_ARE_MAPPED_SUCCESSFULLY", ResourceType.Message);
        public static readonly string ANSWER_INCOMPLETE = ReadResource.GetResource("ANSWER_INCOMPLETE", ResourceType.Message);
        public static readonly string ANSWER_SUBMIT_NOT_VERIFIED = ReadResource.GetResource("ANSWER_SUBMIT_NOT_VERIFIED", ResourceType.Message);
        public static readonly string DUPLICATE_FILE_UPLOAD = ReadResource.GetResource("DUPLICATE_FILE_UPLOAD", ResourceType.Message);
        public static readonly string INITIAL_SUPPLIER_CHECK_VERIFIED = "INITIAL_SUPPLIER_CHECK_VERIFIED";
        public static readonly string ERROR_INITIAL_SUPPLIER_CHECK = "ERROR_INITIAL_SUPPLIER_CHECK";
        public static readonly string INVALID_FILE_TYPE = "INVALID_FILE_TYPE";
        public static readonly string MARKETING_DETAILS_SAVED_SUCCESSFULLY = ReadResource.GetResource("MARKETING_DETAILS_SAVED_SUCCESSFULLY", ResourceType.Message);
        public static readonly string SAVED_SUCCESSFULLY_ANSWERS_NOT_COMPLETE = ReadResource.GetResource("SAVED_SUCCESSFULLY_ANSWERS_NOT_COMPLETE", ResourceType.Message);
        public static readonly string SAVED_SUCCESSFULLY_ANSWERS_COMPLETE = ReadResource.GetResource("SAVED_SUCCESSFULLY_ANSWERS_COMPLETE", ResourceType.Message);
        public static readonly string CLIENT_ANSWERS_SUBMITTED = ReadResource.GetResource("CLIENT_ANSWERS_SUBMITTED", ResourceType.Message);
        public static readonly string PRODUCTS_MARKED_OFFLINE = "PRODUCTS_MARKED_OFFLINE";
        public static readonly string PRODUCTS_SKIPPED_SUCCESSFULLY = "PRODUCTS_SKIPPED_SUCCESSFULLY";
        public static readonly string INVOICE_PAID_SUCCESSFULLY = "INVOICE_PAID_SUCCESSFULLY";
        public static readonly string DEFAULT_PRODUCT_UPDATED_SUCCESSFULLY = "DEFAULT_PRODUCT_UPDATED_SUCCESSFULLY";
        public static readonly string CAMPAIGNS_ASSIGNED_TO_USER = "CAMPAIGNS_ASSIGNED_TO_USER";
        public static readonly string GENERAL_INFORMATION_SAVED_SUCCESFULLY = "GENERAL_INFORMATION_SAVED_SUCCESFULLY";
        public static readonly string CONTACTS_SAVED_SUCCESFULLY = "CONTACTS_SAVED_SUCCESFULLY";
        public static readonly string ENTER_INDUSTRY_SECTOR= "ENTER_INDUSTRY_SECTOR";
        public const string MARKETING_DETAILS_SUCCESS = "MARKETING_DETAILS_SUCCESS";
        public const string SEARCH_FOR_BUYER = "SEARCH_FOR_BUYER";




        #endregion

        #region Mnemonics
        public static readonly string COUNTRY_NAME = "COUNTRY";
        public static readonly string ORG_EMP_BAND = "ORG_EMP_BAND";
        public static readonly string ORG_TURNOVER = "ORG_TURNOVER";
        public static readonly string ORG_BIZ_SECT = "ORG_BIZ_SECT";
        public static readonly string SALES_REGION = "SALES_REGION";
        public static readonly string SERVICE_REGION = "SERVICE_REGION";
        public static readonly string TYPE_COMPANY = "TYPE_COMPANY";

        public static readonly string US_STATES = "US_STATES";
        public static readonly string US_FIRM_STATUS = "US_FIRM_STATUS";
        public static readonly string VERIFY_REASON_CODES = "VERIFY_REASON_CODES";
        public static readonly string FLAGGED_REASON_CODE = "FLAGGED_REASON_CODE";
        #endregion

        #region EmailMnemonics
        public const string USER_ACCOUNT_DETAILS = "USER_ACCOUNT_DETAILS";
        public const string SUBMITTED_FOR_VERIFICATION = "SUBMITTED_FOR_VERIFICATION";
        public const string ADMIN_NOTIFICATION_SUPPLIER_REGISTERED = "ADMIN_NOTIFICATION_SUPPLIER_REGISTERED";
        public const string BUYER_VERIFIED = "BUYER_VERIFIED";
        public const string FORGOT_PASSWORD = "FORGOT_PASSWORD";
        public const string PRE_REG_CAMPAIGN_RELEASE = "PRE_REG_CAMPAIGN_RELEASE";
        public const string VERIFICATION_STATUS_MAIL = "VERIFICATION_STATUS_MAIL";
        public const string RESET_PASSWORD_CONFIRMATION = "RESET_PASSWORD_CONFIRMATION";
        #endregion

        #region Auditor Company Id
        public const int AUDITOR_COMPANY_ID = 10;
        #endregion

        #region Colors

        public const string FlaggedColor = "#FFB84D";
        public const string SelfDeclaredColor = "#D4E2B9";
        public const string VerifiedColor = "#6F9B6B";
        #endregion

        #region ClientSpecific
        public const string BRITVIC_EBP_QuestionSet = "Ethical Business Policy";
        #endregion


        #region
        public const string USER_PREFERENCE = "USER_PREFERENCE";
        public const string USER_PREFERENCE_CULTURE = "USER_PREFERENCE_CULTURE";

        public const string HELP = "HELP";
        public const string ABOUT = "ABOUT";
        public const string TERMS_OF_USE = "TERMS_OF_USE";
        public const string PRIVACY_POLICY = "PRIVACY_POLICY";
        public const string PRGX_GLOBAL_RIGHTS = "PRGX_GLOBAL_RIGHTS";
        public const string HELP_POP_UP_HEADER = "HELP_POP_UP_HEADER";
        public const string EMAIL_US = "EMAIL_US";
        public const string CALL_US = "CALL_US";
        public const string FAQ = "FAQ";
        public const string COOKIES_MESSAGE = "COOKIES_MESSAGE";
        public const string ACCEPT = "ACCEPT";
        public const string READ_MORE = "READ_MORE";
        public const string OR = "OR";
        public const string LOGIN = "LOGIN";
        public const string LOGIN_ID = "LOGIN_ID";
        public const string FORGOT_PASSWORD_LINK = "FORGOT_PASSWORD_LINK";
        public const string FORGOT_PASSWORD_POP_UP_HEADER = "FORGOT_PASSWORD_POP_UP_HEADER";
        public const string FORGOT_PASSWORD_POP_UP_MESSAGE = "FORGOT_PASSWORD_POP_UP_MESSAGE";
        public const string VIEW_SIM_PROFILE = "VIEW_SIM_PROFILE";
        public const string BACK_TO_PAYMENTS = "BACK_TO_PAYMENTS";
        public const string EVALUATION_STARTED = "EVALUATION_STARTED";
        public const string REGISTER_FAQ = "REGISTER_FAQ";
        public const string CANCEL = "CANCEL";
        public const string GET_PASSWORD = "GET_PASSWORD";
        public const string COOKIES = "COOKIES";
        public const string COOKIES_READ_MORE_MESSAGE = "COOKIES_READ_MORE_MESSAGE";
        public const string LOGOUT = "LOGOUT";
        public const string CHANGE_PASSWORD = "CHANGE_PASSWORD";
        public const string HOME = "HOME";
        public const string USERS = "USERS";
        public const string MANAGE_USERS = "MANAGE_USERS";
        public const string DEFINE_ACCESS_TYPES = "DEFINE_ACCESS_TYPES";
        public const string QUESTIONNARIES = "QUESTIONNARIES";
        public const string QUESTIONNARIE_SECTIONS = "QUESTIONNARIE_SECTIONS";
        public const string BUYERS = "BUYERS";
        public const string SUPPLIERS = "SUPPLIERS";
        public const string FINANCE = "FINANCE";
        public const string PAYMENTS = "PAYMENTS";
        public const string VOUCHERS = "VOUCHERS";
        public const string PROFILE = "PROFILE";
        public const string GENERAL_INFORMATION_CONTACTS = "GENERAL_INFORMATION_CONTACTS";
        public const string COMPLIANCE_CHECKS = "COMPLIANCE_CHECKS";
        public const string AWAITING_PAYMENT = "AWAITING_PAYMENT";
        public const string PAYMENT_HISTORY = "PAYMENT_HISTORY";
        public const string INBOX = "INBOX";
        public const string SEARCH = "SEARCH";
        public const string REPORTS = "REPORTS";
        public const string CHECKS = "CHECKS";
        public const string FAQS = "FAQS";
        public const string EMAIL = "EMAIL";
        public const string PHONE = "PHONE";
        public const string NOT_REGISTERED_YET = "NOT_REGISTERED_YET";
        public const string REGISTER_A = "REGISTER_A";
        public const string SELECT_BUYER_SUPPLIER = "SELECT_BUYER_SUPPLIER";
        public const string BUYER = "BUYER";
        public const string SUPPLIER = "SUPPLIER";
        public const string BEGIN_REGISTRATION = "BEGIN_REGISTRATION";
        public const string CONTACT_US = "CONTACT_US";
        public const string CONTACT_US_MESSAGE = "CONTACT_US_MESSAGE";
        public const string WRITE_US = "WRITE_US";
        public const string PRGX_ADDRESS = "PRGX_ADDRESS";
        public const string BACK_TO_TOP = "BACK_TO_TOP";
        public const string SITE_MAP = "SITE_MAP";
        public const string RESET_PASSWORD = "RESET_PASSWORD";
        public const string PASSWORD = "PASSWORD";
        public const string CONFIRM_PASSWORD = "CONFIRM_PASSWORD";
        public const string PASSWORD_RESET_MESSAGE = "PASSWORD_RESET_MESSAGE";
        public const string RETURN_TO_LOGIN = "RETURN_TO_LOGIN";
        public const string PASSWORD_RESET_URL_SENT_MESSAGE = "PASSWORD_RESET_URL_SENT_MESSAGE";
        public const string PASSWORD_TIPSO_MESSAGE = "PASSWORD_TIPSO_MESSAGE";
        public const string USER_EMAIL_NOT_EXISTS = "USER_EMAIL_NOT_EXISTS";
        public const string SELECT_BUYER_OR_SUPPLIER = "SELECT_BUYER_OR_SUPPLIER";
        public const string EVALUATED = "EVALUATED";
        public const string PENDING = "PENDING";
        public const string PUBLISH_PRODUCT_VALIDATION = "PUBLISH_PRODUCT_VALIDATION";
        public const string VALID_DATA_ERROR = "VALID_DATA_ERROR";



        #endregion

        #region FAQ_Constants
        public const string FAQ_DEFINITION = "FAQ_DEFINITION";
        public const string FAQ_QUESTION_1 = "FAQ_QUESTION_1";
        public const string FAQ_QUESTION_2 = "FAQ_QUESTION_2";
        public const string FAQ_QUESTION_3 = "FAQ_QUESTION_3";
        public const string FAQ_QUESTION_4 = "FAQ_QUESTION_4";
        public const string FAQ_QUESTION_5 = "FAQ_QUESTION_5";
        public const string FAQ_QUESTION_6 = "FAQ_QUESTION_6";
        public const string FAQ_QUESTION_7 = "FAQ_QUESTION_7";
        public const string FAQ_QUESTION_8 = "FAQ_QUESTION_8";
        public const string FAQ_QUESTION_9 = "FAQ_QUESTION_9";
        public const string FAQ_QUESTION_10 = "FAQ_QUESTION_10";
        public const string FAQ_QUESTION_11 = "FAQ_QUESTION_11";

        public const string FAQ_ANSWER_1 = "FAQ_ANSWER_1";
        public const string FAQ_ANSWER_2 = "FAQ_ANSWER_2";
        public const string FAQ_ANSWER_3 = "FAQ_ANSWER_3";
        public const string FAQ_ANSWER_4 = "FAQ_ANSWER_4";
        public const string FAQ_ANSWER_5 = "FAQ_ANSWER_5";
        public const string FAQ_ANSWER_6 = "FAQ_ANSWER_6";
        public const string FAQ_ANSWER_7 = "FAQ_ANSWER_7";
        public const string FAQ_ANSWER_8 = "FAQ_ANSWER_8";
        public const string FAQ_ANSWER_9 = "FAQ_ANSWER_9";
        public const string FAQ_ANSWER_10 = "FAQ_ANSWER_10";
        public const string FAQ_ANSWER_11 = "FAQ_ANSWER_11";
        #endregion

        #region Admin_Constants
        public const string ADMIN_HOME = "ADMIN_HOME";
        public const string ADMIN_HOME_SUPPLIER_METRICS_HEADING = "ADMIN_HOME_SUPPLIER_METRICS_HEADING";
        public const string ADMIN_HOME_BUYER_METRICS_HEADING = "ADMIN_HOME_BUYER_METRICS_HEADING";
        public const string ADMIN_HOME_CAMPAIGNS_METRICS_HEADING = "ADMIN_HOME_CAMPAIGNS_METRICS_HEADING";
        public const string ADMIN_HOME_CAMPAIGNS_AWAITING_METRICS_HEADING = "ADMIN_HOME_CAMPAIGNS_AWAITING_METRICS_HEADING";
        public const string QUOTATION_NO = "QUOTATION_NO";
        public const string QUOTATION_DATE = "QUOTATION_DATE";
        public const string PRODUCTS_QUOTATION = "PRODUCTS_QUOTATION";
        public const string PRODUCT_DESC = "PRODUCT_DESC";
        public const string VALUE_GBP = "VALUE_GBP";
        public const string PRICE_NET = "PRICE_NET";
        public const string DISCOUNT_NET = "DISCOUNT_NET";
        public const string SUBTOTAL_NET = "SUBTOTAL_NET";
        public const string SERVICE_PROVIDER = "SUBTOTAL_NET";
        public const string PRGX_UK_LTD = "PRGX_UK_LTD";
        public const string REG_ADDRESS = "REG_ADDRESS";
        public const string PRGX_REG_ADDRESS_LINE1 = "PRGX_REG_ADDRESS_LINE1";
        public const string PRGX_REG_ADDRESS_LINE2 = "PRGX_REG_ADDRESS_LINE2";
        public const string PRGX_REG_ADDRESS_LINE3 = "PRGX_REG_ADDRESS_LINE3";
        public const string PRGX_COMPANY_REG_NO = "PRGX_COMPANY_REG_NO";
        public const string PRGX_VAT_NO = "PRGX_VAT_NO";
        public const string PRGX_TELEPHONE_NO = "PRGX_TELEPHONE_NO";
        public const string PRGX_EMAIL = "PRGX_EMAIL";
        public const string PAYABLE_BY = "PAYABLE_BY";
        public const string REGISTRANT_DETAILS = "REGISTRANT_DETAILS";
        public const string BANK_DETAILS_FOR_BACS_PAYMENT = "BANK_DETAILS_FOR_BACS_PAYMENT";
        public const string BACS_BANK_DETAILS_LINE1 = "BACS_BANK_DETAILS_LINE1";
        public const string BACS_BANK_DETAILS_LINE2 = "BACS_BANK_DETAILS_LINE2";
        public const string BACS_BANK_ACC_NO = "BACS_BANK_ACC_NO";
        public const string BACS_BANK_SORT_CODE = "BACS_BANK_SORT_CODE";
        public const string BACS_BANK_IBAN_NO = "BACS_BANK_IBAN_NO";
        public const string PAYMENT_REF_SIM = "PAYMENT_REF_SIM";
        public const string CERTIFICATES_LICENCES = "CERTIFICATES_LICENCES";
        public const string BACK_TO_ALL_BUYER_ORG = "BACK_TO_ALL_BUYER_ORG";
        public const string NOTES_ERROR = "NOTES_ERROR";
        public const string DATA_SOURCE_ERROR = "DATA_SOURCE_ERROR";
        public const string NOTES_AND_DATA_SOURCE_ERROR = "NOTES_AND_DATA_SOURCE_ERROR";
        public const string MASTER_VENDOR_ERROR = "MASTER_VENDOR_ERROR";
        public const string SUPPLIER_COUNT_ERROR = "SUPPLIER_COUNT_ERROR";
        public const string COMPARE_MASTER_VENDOR_AND_SUPP_COUNT = "COMPARE_MASTER_VENDOR_AND_SUPP_COUNT";
        public const string BUYER_ORG_ERROR = "BUYER_ORG_ERROR";
        public const string CAMPAIGN_TEMPLATE_ERROR = "CAMPAIGN_TEMPLATE_ERROR";
        public const string CAMPAIGN_URL_ERROR = "CAMPAIGN_URL_ERROR";
        public const string CAMPAIGN_DETAILS = "CAMPAIGN_DETAILS";
        public const string CAMPAIGN_NAME_ERROR = "CAMPAIGN_NAME_ERROR";
        public const string CAMPAIGN_NAME_DISPLAY = "CAMPAIGN_NAME_DISPLAY";
        public const string BUYER_ORG_DISPLAY = "BUYER_ORG_DISPLAY";
        public const string NO_OF_SUPPLIERS_DISPLAY = "NO_OF_SUPPLIERS_DISPLAY";
        public const string NO_OF_SUPPLIERS_ERROR = "NO_OF_SUPPLIERS_ERROR";
        public const string MASTER_VENDOR_DISPLAY = "MASTER_VENDOR_DISPLAY";
        public const string CAMPAIGN_URL_DISPLAY = "CAMPAIGN_URL_DISPLAY";
        public const string WELCOME_MESSAGE_DISPLAY = "WELCOME_MESSAGE_DISPLAY";
        public const string VOUCHER_DISPLAY = "VOUCHER_DISPLAY";
        public const string NOTE_DISPLAY = "NOTE_DISPLAY";
        public const string DATA_SOURCE_DISPLAY = "DATA_SOURCE_DISPLAY";
        public const string CAMPAIGN_TYPE_DISPLAY = "CAMPAIGN_TYPE_DISPLAY";
        public const string CAMPAIGN_TYPE_ERROR = "CAMPAIGN_TYPE_ERROR";
        public const string PAGE_TEMPLATE_DISPLAY = "PAGE_TEMPLATE_DISPLAY";
        public const string EMAIL_CONTENT_DISPLAY = "EMAIL_CONTENT_DISPLAY";
        public const string PRE_REG_FILE_DISPLAY = "PRE_REG_FILE_DISPLAY";
        public const string BACK_TO_MANAGE_QUESTIONNAIRE = "BACK_TO_MANAGE_QUESTIONNAIRE";
        public const string CREATE_QUESTIONNAIRE_SECTION = "CREATE_QUESTIONNAIRE_SECTION";
        public const string CREATE_NEW_QUESTIONNAIRE_SECTION = "CREATE_NEW_QUESTIONNAIRE_SECTION";
        public const string EDIT_QUESTIONNAIRE_SECTION = "EDIT_QUESTIONNAIRE_SECTION";
        public const string BACK_TO_ALL_QUESTIONNAIRE_SECTION = "BACK_TO_ALL_QUESTIONNAIRE_SECTION";
        public const string NAME_DISPLAY = "NAME_DISPLAY";
        public const string QUESTIONNAIRE_NAME_ERROR = "QUESTIONNAIRE_NAME_ERROR";
        public const string DESCRIPTION_DISPLAY = "DESCRIPTION_DISPLAY";
        public const string QUESTIONNAIRE_DESCRIPTION_ERROR = "QUESTIONNAIRE_DESCRIPTION_ERROR";
        public const string IS_SAVE_NEEDED_DISPLAY = "IS_SAVE_NEEDED_DISPLAY";
        public const string CREATE_USER = "CREATE_USER";
        public const string IS_ADMIN_BUYER = "IS_ADMIN_BUYER";
        public const string EDIT_USER = "EDIT_USER";
        public const string MANAGE_QUESTIONNAIRE_SECTION = "MANAGE_QUESTIONNAIRE_SECTION";
        public const string NOT_PUBLISHED = "NOT_PUBLISHED";
        public const string QUESTIONNAIRE_SECTION = "QUESTIONNAIRE_SECTION";
        public const string MAPPED = "MAPPED";
        public const string EDIT_SECTION_DETAILS = "EDIT_SECTION_DETAILS";
        public const string CREATE_NEW_QUESTION = "CREATE_NEW_QUESTION";
        public const string PUBLISH_SECTION_AND_QUESTIONS = "PUBLISH_SECTION_AND_QUESTIONS";
        public const string NA = "NA";
        public const string FINANCE_INSURANCE_AND_TAX = "FINANCE_INSURANCE_AND_TAX";
        public const string ADD_USER = "ADD_USER";
        public const string BUYER_ORGANISATIONS = "BUYER_ORGANISATIONS";
        public const string SEARCH_FOR_BUYER_ORG = "SEARCH_FOR_BUYER_ORG";
        public const string VERIFIED_REG = "VERIFIED_REG";
        public const string VERIFIED_AND_ACTIVATED = "VERIFIED_AND_ACTIVATED";
        public const string ACCESS_TYPE = "ACCESS_TYPE";
        public const string PRIMARY_CONTACT = "PRIMARY_CONTACT";
        public const string REQUEST = "REQUEST";
        public const string TERMS_ACCEPTED = "TERMS_ACCEPTED";
        public const string ACTIVATED = "ACTIVATED";
        public const string ASSIGN_PRODUCT = "ASSIGN_PRODUCT";
        public const string ASSIGN_PRODUCTS = "ASSIGN_PRODUCTS";
        public const string ASSIGN_TO_AUDITOR = "ASSIGN_TO_AUDITOR";
        public const string VIEW_CAMPAIGN = "VIEW_CAMPAIGN";
        public const string MAP_TO_BUYER = "MAP_TO_BUYER";
        public const string UNMAP_FROM_BUYER = "UNMAP_FROM_BUYER";
        public const string EDIT_USER_DETAILS = "EDIT_USER_DETAILS";
        public const string MAP_QUESTIONNAIRE_SECTION = "MAP_QUESTIONNAIRE_SECTION";
        public const string UNMAP_QUESTIONNAIRE_SECTION = "UNMAP_QUESTIONNAIRE_SECTION";
        public const string PRODUCT_NAME = "PRODUCT_NAME";
        public const string COMPANY_NAME_DISPLAY = "COMPANY_NAME_DISPLAY";
        public const string CHOOSE_ACCESS_TYPE_DISPLAY = "CHOOSE_ACCESS_TYPE_DISPLAY";
        public const string NONE_SELECTED = "NONE_SELECTED";
        public const string CHANGE_ACCESS_TYPE = "CHANGE_ACCESS_TYPE";
        public const string CREATE_CAMPAIGN = "CREATE_CAMPAIGN";
        public const string EXPORT_SUPPLIER_LIST = "EXPORT_SUPPLIER_LIST";
        public const string BUYER_SUPPLIER_MAPPING = "BUYER_SUPPLIER_MAPPING";
        public const string CHANGE_ACCESS = "CHANGE_ACCESS";
        public const string UPDATE = "UPDATE";
        public const string BUYER_ACCESS_ERROR = "BUYER_ACCESS_ERROR";
        public const string DEFAULT_PRODUCTS = "DEFAULT_PRODUCTS";
        public const string COMPANY_DETAILS = "COMPANY_DETAILS";
        public const string MARKETING = "MARKETING";
        public const string BANK_DETAILS = "BANK_DETAILS";
        public const string REFERENCES = "REFERENCES";
        public const string BACK_TO_QUESTIONNAIRE_SUMMARY = "BACK_TO_QUESTIONNAIRE_SUMMARY";
        public const string QUESTIONNAIRE_SUMMARY = "QUESTIONNAIRE_SUMMARY";
        public const string SAVE_ANSWERS = "SAVE_ANSWERS";
        public const string ADD_CONTACT = "ADD_CONTACT";
        public const string ADD_ADDRESS = "ADD_ADDRESS";
        public const string ADD_BANK_DETAILS = "ADD_BANK_DETAILS";
        public const string REFERENCE_DETAILS = "REFERENCE_DETAILS";
        public const string ADD_REFERENCE_DETAILS = "ADD_REFERENCE_DETAILS";
        public const string ADD_REFERENCES = "ADD_REFERENCES";
        public const string ANSWER_THIS_SECTION = "ANSWER_THIS_SECTION";
        public const string CONTACTS_MESSAGE = "CONTACTS_MESSAGE";
        public const string ADDRESSES_MESSAGE = "ADDRESSES_MESSAGE";
        public const string MARKETING_MESSAGE = "MARKETING_MESSAGE";
        public const string CAPABILITIES_MESSAGE = "CAPABILITIES_MESSAGE";
        public const string BANK_DETAILS_MESSAGE = "BANK_DETAILS_MESSAGE";
        public const string REFERENCES_MESSAGE = "REFERENCES_MESSAGE";
        public const string TITLE = "TITLE";
        public const string MAILING_ADDRESS = "MAILING_ADDRESS";
        public const string FAX = "FAX";
        public const string RELATIONSHIP_ROLE = "RELATIONSHIP_ROLE";
        public const string ASSIGNED_CLIENTS = "ASSIGNED_CLIENTS";
        public const string ASSIGN_TO_CLIENTS = "ASSIGN_TO_CLIENTS";
        public const string ADDRESS_TYPE = "ADDRESS_TYPE";
        public const string SELECT_ADDRESS_TYPE = "SELECT_ADDRESS_TYPE";
        public const string MAILING = "MAILING";
        public const string HEADQUARTERS = "HEADQUARTERS";
        public const string REMITTANCE = "REMITTANCE";
        public const string BRANCH = "BRANCH";
        public const string THIRD_PARTY = "THIRD_PARTY";
        public const string ADDRESS_TYPE_ERROR = "ADDRESS_TYPE_ERROR";
        public const string SELECT_ROLE = "SELECT_ROLE";
        public const string SALES = "SALES";
        public const string ACC_FIN_REMITTANCE = "ACC_FIN_REMITTANCE";
        public const string SUSTAINABILITY = "SUSTAINABILITY";
        public const string PROCUREMENT = "PROCUREMENT";
        public const string SELECT_RELATIONSHIP = "SELECT_RELATIONSHIP";
        public const string EDIT_ASSIGNMENT = "EDIT_ASSIGNMENT";
        public const string ASSIGN_ADDRESS = "ASSIGN_ADDRESS";
        public const string ASSIGNED = "ASSIGNED";
        public const string ACCOUNT_NAME = "ACCOUNT_NAME";
        public const string ACCOUNT_NO = "ACCOUNT_NO";
        public const string SORT_CODE = "SORT_CODE";
        public const string SWIFT_CODE = "SWIFT_CODE";
        public const string IBAN = "IBAN";
        public const string BANK_NAME = "BANK_NAME";
        public const string PREFERRED_MODE = "PREFERRED_MODE";
        public const string ACCOUNT_NAME_ERROR = "ACCOUNT_NAME_ERROR";
        public const string ACCOUNT_NO_ERROR = "ACCOUNT_NO_ERROR";
        public const string SORT_CODE_ERROR = "SORT_CODE_ERROR";
        public const string SWIFT_CODE_ERROR = "SWIFT_CODE_ERROR";
        public const string IBAN_ERROR = "IBAN_ERROR";
        public const string BANK_NAME_ERROR = "BANK_NAME_ERROR";
        public const string BANK_ADDRESS = "BANK_ADDRESS";
        public const string BANK_ADDRESS_ERROR = "BANK_ADDRESS_ERROR";
        public const string PREFERRED_PAYMENT_METHOD = "PREFERRED_PAYMENT_METHOD";
        public const string PREFERRED_PAYMENT_METHOD_ERROR = "PREFERRED_PAYMENT_METHOD_ERROR";
        public const string CLIENT_NAME = "CLIENT_NAME";
        public const string CONTACT_NAME = "CONTACT_NAME";
        public const string CLIENT_ROLE = "CLIENT_ROLE";
        public const string CAN_WE_CONTACT = "CAN_WE_CONTACT";
        public const string CLIENT_NAME_ERROR = "CLIENT_NAME_ERROR";
        public const string CONTACT_NAME_ERROR = "CONTACT_NAME_ERROR";
        public const string FAX_ERROR = "FAX_ERROR";
        public const string CLIENT_ROLE_ERROR = "CLIENT_ROLE_ERROR";
        public const string SELECT_TO_CONTACT_OR_NOT = "SELECT_TO_CONTACT_OR_NOT";
        public const string INFO = "INFO";
        public const string QUESTIONNAIRE_LOWER_CASE = "QUESTIONNAIRE_LOWER_CASE";
        public const string IS_ASSIGNED_TO = "IS_ASSIGNED_TO";
        public const string DELETE_CONTACT_CONFIRMATION = "DELETE_CONTACT_CONFIRMATION";
        public const string IS_DELETED_SUCCESSFULLY = "IS_DELETED_SUCCESSFULLY";
        public const string NO_ROLE = "NO_ROLE";
        public const string EDIT_CONTACT = "EDIT_CONTACT";
        public const string SELECT_ADDRESS = "SELECT_ADDRESS";
        public const string ADD_NEW_ADDRESS = "ADD_NEW_ADDRESS";
        public const string ALREADY_MAPPED_FOR = "ALREADY_MAPPED_FOR";
        public const string MAPPED_CONTACT_TEXT = "MAPPED_CONTACT_TEXT";
        public const string MAP_AND_UNMAP = "MAP_AND_UNMAP";
        public const string UPDATE_CONTACT_SUCCESS = "UPDATE_CONTACT_SUCCESS";
        public const string ADD_CONTACT_SUCCESS = "ADD_CONTACT_SUCCESS";
        public const string ENTER_VALID_EMAIL = "ENTER_VALID_EMAIL";
        public const string ENTER_VALID_PHONE_NO = "ENTER_VALID_PHONE_NO";
        public const string CANNOT_SELECT_FIRM_DIVERSITY = "CANNOT_SELECT_FIRM_DIVERSITY";
        public const string EDIT_ADDRESS = "EDIT_ADDRESS";
        public const string SELECT_COUNTRY = "SELECT_COUNTRY";
        public const string ADDRESS_ALREADY_MAPPED = "ADDRESS_ALREADY_MAPPED";
        public const string MAP_EXISTING_ADDRESS = "MAP_EXISTING_ADDRESS";
        public const string UPDATE_ADDRESS_SUCCESS = "UPDATE_CONTACT_SUCCESS";
        public const string ADD_ADDRESS_SUCCESS = "ADD_CONTACT_SUCCESS";
        public const string DELETE_ADDRESS_CONFIRMATION = "DELETE_ADDRESS_CONFIRMATION";
        public const string DELETE_ADDRESS_SUCCESS = "DELETE_ADDRESS_SUCCESS";
        public const string ASSIGN_ADDRESS_SUCCESS = "ASSIGN_ADDRESS_SUCCESS";
        public const string UNASSIGN_ADDRESS_SUCCESS = "UNASSIGN_ADDRESS_SUCCESS";
        public const string COMPANY_DETAILS_SUCCESS = "COMPANY_DETAILS_SUCCESS";
        public const string ASSIGN_BANK_DETAILS_SUCCESS = "ASSIGN_BANK_DETAILS_SUCCESS";
        public const string UNASSIGN_BANK_DETAILS_SUCCESS = "UNASSIGN_BANK_DETAILS_SUCCESS";
        public const string EDIT_BANK_DETAILS = "EDIT_BANK_DETAILS";
        public const string ADD_BANK_DETAILS_SUCCESS = "ADD_BANK_DETAILS_SUCCESS";
        public const string UPDATE_BANK_DETAILS_SUCCESS = "UPDATE_BANK_DETAILS_SUCCESS";
        public const string DELETE_ACCOUNT_CONFIRMATION = "DELETE_ACCOUNT_CONFIRMATION";
        public const string EDIT_REFERENCE_DETAILS = "EDIT_REFERENCE_DETAILS";
        public const string UPDATE_REFERENCE_DETAILS_SUCCESS = "UPDATE_REFERENCE_DETAILS_SUCCESS";
        public const string ADD_REFERENCE_DETAILS_SUCCESS = "ADD_REFERENCE_DETAILS_SUCCESS";
        public const string DELETE_REFERENCE_CONFIRMATION = "DELETE_REFERENCE_CONFIRMATION";
        public const string ASSIGN_REFERENCE_SUCCESS = "ASSIGN_REFERENCE_SUCCESS";
        public const string UNASSIGN_REFERENCE_SUCCESS = "UNASSIGN_REFERENCE_SUCCESS";
        public const string CAPABILITY_DETAILS_SUCCESS = "CAPABILITY_DETAILS_SUCCESS";
        public const string LANDING_PAGE_REFERRER = "LANDING_PAGE_REFERRER";
        public const string OTHER_REFERRER = "OTHER_REFERRER";
        public const string DETAILS_VERIFIED_DATE = "DETAILS_VERIFIED_DATE";
        public const string PROFILE_VERIFIED_DATE = "PROFILE_VERIFIED_DATE";
        public const string SANCTION_VERIFIED_DATE = "SANCTION_VERIFIED_DATE";
        #endregion

        #region Common_Constants
        public const string SOURCE = "SOURCE";
        public const string SECTIONS = "SECTIONS";
        public const string REFERRER = "REFERRER";
        public const string STATUS = "STATUS";
        public const string PRIMARY_CONTACT_NAME = "PRIMARY_CONTACT_NAME";
        public const string PRIMARY_CONTACT_EMAIL = "PRIMARY_CONTACT_EMAIL";
        public const string VERIFIED = "VERIFIED";
        public const string ACCESS = "ACCESS";
        public const string ACTIONS = "ACTIONS";
        public const string VERIFY_BUYER = "VERIFY_BUYER";
        public const string ACTIVATE_BUYER = "ACTIVATE_BUYER";
        public const string VERIFY = "VERIFY";
        public const string APPROVE = "APPROVE";
        public const string RELEASE = "RELEASE";
        public const string BUYER_ORGANISATION = "BUYER_ORGANISATION";
        public const string CAMPAIGN_NAME = "CAMPAIGN_NAME";
        public const string CAMPAIGN_TYPE = "CAMPAIGN_TYPE";
        public const string CREATED_DATE = "CREATED_DATE";
        public const string SUPPLIER_COUNT = "SUPPLIER_COUNT";
        public const string BUYER_NAME = "BUYER_NAME";
        public const string NO_OF_SUPPLIERS = "NO_OF_SUPPLIERS";
        public const string WORKSHEET = "WORKSHEET";
        public const string REVERT = "REVERT";
        public const string MAIL_VIA_SILVER_POP = "MAIL_VIA_SILVER_POP";
        public const string SILVER_POP_LIST = "SILVER_POP_LIST";
        public const string CAMPAIGN_STATUS = "CAMPAIGN_STATUS";
        public const string ASSIGNED_TO_AUDITOR = "ASSIGNED_TO_AUDITOR";
        public const string ASSIGN_AUDITOR = "ASSIGN_AUDITOR";
        public const string CREATED_NAME = "CREATED_NAME";
        public const string APPROVE_CAMPAIGN = "APPROVE_CAMPAIGN";
        public const string ARE_YOU_SURE_YOU_WANT_TO_APPROVE_THIS_CAMPAIGN = "ARE_YOU_SURE_YOU_WANT_TO_APPROVE_THIS_CAMPAIGN";
        public const string RELEASE_CAMPAIGN = "RELEASE_CAMPAIGN";
        public const string PUBLISH_PRODUCTS = "PUBLISH_PRODUCTS";
        public const string PUBLISH_PRODUCTS_MESSAGE = "PUBLISH_PRODUCTS_MESSAGE";
        public const string IS_ADMIN_SUPPLIER = "IS_ADMIN_SUPPLIER";
        public const string PRODUCT = "PRODUCT";
        public const string SELECT_AUDITOR = "SELECT_AUDITOR";
        public const string SELECT_CAMPAIGN_AUDITOR = "SELECT_CAMPAIGN_AUDITOR";
        public const string ASSIGN = "ASSIGN";
        public const string ASSIGN_ACCESS = "ASSIGN_ACCESS";
        public const string CHOOSE_ACCESS_TYPE = "CHOOSE_ACCESS_TYPE";
        public const string ACTIVATE = "ACTIVATE";
        public const string PUBLISHED = "PUBLISHED";
        public const string NO_RECORDS_FOUND = "NO_RECORDS_FOUND";
        public const string SEARCH_PAGE_DATA = "SEARCH_PAGE_DATA";
        public const string ERROR_MESSAGE = "ERROR_MESSAGE";
        public const string PUBLISHED_SUCCESSFULLY = "PUBLISHED_SUCCESSFULLY";
        public const string NO_PRODUCT_SELECTED_TO_PUBLISH = "NO_PRODUCT_SELECTED_TO_PUBLISH";
        public const string CAMPAIGN_ASSIGNED_TO_AUDITOR_SUCCESS_MESSAGE = "CAMPAIGN_ASSIGNED_TO_AUDITOR_SUCCESS_MESSAGE";
        public const string CAMPAIGN_RELEASED_SUCCESS_MESSAGE = "CAMPAIGN_RELEASED_SUCCESS_MESSAGE";
        public const string CAMPAIGN_ASSIGNED_TO_AUDITOR_ERROR_MESSAGE = "CAMPAIGN_ASSIGNED_TO_AUDITOR_ERROR_MESSAGE";
        public const string CAMPAIGN_RELEASED_ERROR_MESSAGE = "CAMPAIGN_RELEASED_ERROR_MESSAGE";
        public const string REVERT_CAMPAIGN_SUCCESS_MESSAGE = "REVERT_CAMPAIGN_SUCCESS_MESSAGE";
        public const string REVERT_CAMPAIGN_ERROR_MESSAGE = "REVERT_CAMPAIGN_ERROR_MESSAGE";
        public const string SUPPLIER_COUNT_EXCEEDS_MASTER_VENDOR_VALUE = "SUPPLIER_COUNT_EXCEEDS_MASTER_VENDOR_VALUE";
        public const string PLEASE_SELECT_BUYER_ACCESS = "PLEASE_SELECT_BUYER_ACCESS";
        public const string BUYER_ACTIVATED_SUCCESSFULLY_SUCCESS_MESSAGE = "BUYER_ACTIVATED_SUCCESSFULLY_SUCCESS_MESSAGE";
        public const string CAMPAIGN_APPROVED_SUCCESS_MESSAGE = "CAMPAIGN_APPROVED_SUCCESS_MESSAGE";
        public const string UPDATE_MASTER_VENDOR_ERROR_MESSAGE = "UPDATE_MASTER_VENDOR_ERROR_MESSAGE";
        public const string RELEASE_CAMPAIGN_WITHOUT_MAIL = "RELEASE_CAMPAIGN_WITHOUT_MAIL";
        public const string RELEASE_CAMPAIGN_SUGGESTIONS = "RELEASE_CAMPAIGN_SUGGESTIONS";
        public const string CONFIRM_RELEASE_CAMPAIGN = "CONFIRM_RELEASE_CAMPAIGN";
        public const string DOWNLOAD = "DOWNLOAD";
        public const string VERIFY_LIST = "VERIFY_LIST";
        public const string PLEASE_READ_AND_AGREE_TO_THE_TERMS_AND_CONDITIONS = "PLEASE_READ_AND_AGREE_TO_THE_TERMS_AND_CONDITIONS";


        public const string SIM_AND_CIPS = "SIM_AND_CIPS";
        public const string SIM_ONLY = "SIM_ONLY";
        public const string CIPS_ONLY = "CIPS_ONLY";
        public const string ALL = "ALL";
        public const string SANCTION = "SANCTION";
        public const string FIT = "FIT";
        public const string HS = "HS";
        public const string DS = "DS";


        public const string SID = "SID";
        public const string COMPANY_NAME = "COMPANY_NAME";
        public const string DETAILS = "DETAILS";
        public const string PUBLISH = "PUBLISH";
        public const string CHECKED = "CHECKED";
        public const string CHECK = "CHECK";
        public const string SIM = "SIM";
        public const string CIPS = "CIPS";
        public const string EXPORT = "EXPORT";
        public const string EDIT_CAMPAIGN = "EDIT_CAMPAIGN";
        public const string SUMMARY = "SUMMARY";
        public const string STAGE = "STAGE";
        public const string SUPPLIERS_YOU_TRADE = "SUPPLIERS_YOU_TRADE";

        public const string FAVOURITE_SUPPLIERS = "FAVOURITE_SUPPLIERS";

        public const string ANY_SHARING = "ANY_SHARING";

        public const string ALL_SHARED = "ALL_SHARED";

        public const string SOME_SHARED = "SOME_SHARED";

        public const string NO_SHARED = "NO_SHARED";
        public const string ACTION = "ACTION";
        public const string SEND_MESSAGE = "SEND_MESSAGE";
        public const string SEND = "SEND";

        public const string MESSAGES_FROM = "MESSAGES_FROM";
        public const string LAST_MESSAGE_SENT_RECIEVED = "LAST_MESSAGE_SENT_RECIEVED";
        public const string ANSWERS_SHARED_WITH_YOU = "ANSWERS_SHARED_WITH_YOU";
        public const string REGISTRATION = "REGISTRATION";

        public const string FINANCE_INSURANCE_TAX = "FINANCE_INSURANCE_TAX";
        public const string HEALTH_SAFETY = "HEALTH_SAFETY";
        public const string DATA_SECURITY = "DATA_SECURITY";
        public const string RESULTS_PER_PAGE = "RESULTS_PER_PAGE";
        public const string DISCUSSION = "DISCUSSION";
        public const string VIEW_SUPPLIER_PROFILE = "VIEW_SUPPLIER_PROFILE";
        public const string SHOW_OLDER_MESSAGES = "SHOW_OLDER_MESSAGES";
        public const string CURRENT_ACCESS_TO_ANSWERS = "CURRENT_ACCESS_TO_ANSWERS";
        public const string VIEW_DISCUSSION = "VIEW_DISCUSSION";

        public const string NOT_SHARED = "NOT_SHARED";
        public const string SHARED = "SHARED";
        public const string NONE = "NONE";
        public const string SOME = "SOME";
        public const string START_DISCUSSION = "START_DISCUSSION";
        public const string ACTION_SELECTED = "ACTION_SELECTED";
        public const string MESSAGE_SENT_SUCCESSFULLY = "MESSAGE_SENT_SUCCESSFULLY";
        public const string FOLDERS = "FOLDERS";
        public const string MESSAGE = "MESSAGE";
        //public const string SUPPLIER_YOU_TRADE = "SUPPLIER_YOU_TRADE";

        //public const string FAVOURITE_SUPPLIER = "FAVOURITE_SUPPLIER";
        public const string CREATE_NEW_AUDITOR = "CREATE_NEW_AUDITOR";
        public const string FILTERS = "FILTERS";

        public const string USER_NAME = "USER_NAME";
        public const string USER_TYPE = "USER_TYPE";
        public const string LATEST_TERMS = "LATEST_TERMS";
        public const string LAST_LOGIN = "LAST_LOGIN";
        public const string CREATE_AUDITOR = "CREATE_AUDITOR";
        public const string SAVE = "SAVE";
        public const string DELETE = "DELETE";
        public const string DELETE_AUDITOR = "DELETE_AUDITOR";
        public const string DELETE_RECORD_CONFIRMATION = "DELETE_RECORD_CONFIRMATION";
        public const string SAVE_CHANGES = "SAVE_CHANGES";
        public const string RESET = "RESET";
        public const string CHANGE_PASSWORD_DISPLAY_MESSAGE = "CHANGE_PASSWORD_DISPLAY_MESSAGE";
        public const string EDIT_PASSWORD = "EDIT_PASSWORD";
        public const string EDIT_USER_PROFILE = "EDIT_USER_PROFILE";
        public const string ASSIGN_ROLES = "ASSIGN_ROLES";
        public const string CLEAR_ALL = "CLEAR_ALL";
        public const string BOTH = "BOTH";
        public const string ACTIVE = "ACTIVE";
        public const string DISABLED = "DISABLED";
        public const string FIRST_NAME = "FIRST_NAME";
        public const string LAST_NAME = "LAST_NAME";
        public const string ROLES = "ROLES";
        public const string FIRST_OR_LAST_NAME = "FIRST_OR_LAST_NAME";
        public const string NAME = "NAME";
        public const string REQUIRED_FIELDS_VALIDATION = "REQUIRED_FIELDS_VALIDATION";
        public const string EMAIL_ALREADY_EXISTS = "EMAIL_ALREADY_EXISTS";
        public const string CREATE_BUYER_ROLE = "CREATE_BUYER_ROLE";
        public const string EDIT_BUYER_ROLE = "EDIT_BUYER_ROLE";
        public const string CREATE_AUDITOR_ROLE = "CREATE_AUDITOR_ROLE";
        public const string EDIT_AUDITOR_ROLE = "EDIT_AUDITOR_ROLE";
        public const string CREATE_NEW = "CREATE_NEW";
        public const string TYPE = "TYPE";
        public const string ACCESS_TYPES = "ACCESS_TYPES";
        public const string ROLE_NAME = "ROLE_NAME";
        public const string DESCRIPTION = "DESCRIPTION";
        public const string DELETE_AUDITOR_ROLE = "DELETE_AUDITOR_ROLE";
        public const string DELETE_BUYER_ROLE = "DELETE_BUYER_ROLE";
        public const string DELETE_AUDITOR_ROLE_CONFIRMATION = "DELETE_AUDITOR_ROLE_CONFIRMATION";
        public const string DELETE_BUYER_ROLE_CONFIRMATION = "DELETE_BUYER_ROLE_CONFIRMATION";
        public const string ACCESS_CONFIGURATION = "ACCESS_CONFIGURATION";
        public const string ACCESS_CONFIGURATION_VALIDATION = "ACCESS_CONFIGURATION_VALIDATION";
        public const string ROLE_DESCRIPTION_VALIDATION = "ROLE_DESCRIPTION_VALIDATION";
        public const string ROLE_NAME_VALIDATION = "ROLE_NAME_VALIDATION";
        public const string NAVIGATE = "NAVIGATE";
        public const string BUYER_ROLE_ADDED_SUCCESSFULLY = "BUYER_ROLE_ADDED_SUCCESSFULLY";
        public const string BUYER_ROLE_UPDATED_SUCCESSFULLY = "BUYER_ROLE_UPDATED_SUCCESSFULLY";
        public const string BUSINESS_SECTOR = "BUSINESS_SECTOR";
        public const string COMPANY_TURNOVER = "COMPANY_TURNOVER";
        public const string CUSTOMER_SECTOR = "CUSTOMER_SECTOR";
        public const string SERVICE_IN = "SERVICE_IN";
        public const string SUBSIDIARIES_IN = "SUBSIDIARIES_IN";
        public const string TRADING_NAME = "TRADING_NAME";
        public const string REGISTERED_COUNTRY = "REGISTERED_COUNTRY";
        public const string MAX_CONTRACT = "MAX_CONTRACT";
        public const string MIN_CONTRACT = "MIN_CONTRACT";


        public const string PAYMENT = "PAYMENT";
        public const string CREDIT_NOTE = "CREDIT_NOTE";
        public const string GENERATE_QUOTATION = "GENERATE_QUOTATION";
        public const string AUTHORISE_TRANSACTION = "AUTHORISE_TRANSACTION";
        public const string FROM_DATE = "FROM_DATE";
        public const string TO_DATE = "TO_DATE";
        public const string ORGANISATION = "ORGANISATION";
        public const string CREDIT_ID = "CREDIT_ID";
        public const string SIMR = "SIMR";
        // public const string SUPPLIER_ID = "SUPPLIER_ID";
        public const string INVOICE_STATUS = "INVOICE_STATUS";
        public const string COUNTRY = "COUNTRY";
        public const string AUTHORISED_INVOICES = "AUTHORISED_INVOICES";
        public const string NUMBER_OF_SUPPLIERS = "NUMBER_OF_SUPPLIERS";
        public const string NO_OF_PAYMENTS = "NO_OF_PAYMENTS";
        // public const string SUPPLIER_NAME = "SUPPLIER_NAME";
        public const string PRICE = "PRICE";
        public const string DISC = "DISC";
        public const string SUB = "SUB";
        // public const string VAT = "VAT";
        public const string TOTAL = "TOTAL";
        public const string CURRENCY = "CURRENCY";
        public const string VOUCHER = "VOUCHER";
        public const string TRANSACTION = "TRANSACTION";
        public const string PRODUCTS = "PRODUCTS";
        public const string SELECT_PRODUCT = "SELECT_PRODUCT";
        public const string ALLOW_INVOICE_OR_SKIP = "ALLOW_INVOICE_OR_SKIP";
        public const string DATE_PAID = "DATE_PAID";
        public const string MARK_AS_PAID = "MARK_AS_PAID";
        public const string DISCOUNT_PERCENT = "DISCOUNT_PERCENT";
        public const string PAID_INVOICE = "PAID_INVOICE";
        public const string SELECT = "SELECT";
        public const string INVOICE = "INVOICE";
        public const string DISCOUNT = "DISCOUNT";
        public const string DATE_OF_REFUND = "DATE_OF_REFUND";
        public const string REFUND_METHOD = "REFUND_METHOD";
        public const string COMMENTS_VISIBLE_TO_SUPPLIER = "COMMENTS_VISIBLE_TO_SUPPLIER";
        public const string REFUND_NET = "REFUND_NET";
        public const string VAT_AMOUNT = "VAT_AMOUNT";
        public const string REMOVE = "REMOVE";
        public const string CREATE = "CREATE";
        public const string EDIT_VOUCHER = "EDIT_VOUCHER";
        public const string NOT_MAPPED = "NOT_MAPPED";



        public const string NUMERIC_VALUES_VALIDATION = "NUMERIC_VALUES_VALIDATION";
        public const string DECIMAL_VALUES_VALIDATION = "DECIMAL_VALUES_VALIDATION";
        public const string DATE_VALIDATION = "DATE_VALIDATION";
        public const string SELECT_EXISTING_COMPANY = "SELECT_EXISTING_COMPANY";
        public const string INVOICE_ALLOWED = "INVOICE_ALLOWED";
        public const string NO_PRODUCTS_FOR_PAYMENT = "NO_PRODUCTS_FOR_PAYMENT";
        public const string SELECT_PRODUCTS_FOR_PAYMENT = "SELECT_PRODUCTS_FOR_PAYMENT";
        public const string SELECT_DATE_PAID = "SELECT_DATE_PAID";
        public const string SUPPLIER_NAME_VALIDATION = "SUPPLIER_NAME_VALIDATION";
        public const string PRODUCT_SELECT = "PRODUCT_SELECT";
        public const string DISCOUNT_PERCENT_VALIDATION = "DISCOUNT_PERCENT_VALIDATION";
        public const string BANK_TRANSFER = "BANK_TRANSFER";
        public const string ONLINE_REFUND = "ONLINE_REFUND";
        public const string CREDIT_NOTE_ALREADY_EXISTS = "CREDIT_NOTE_ALREADY_EXISTS";
        public const string NO_INVOICE = "NO_INVOICE";
        public const string ENTER_ALL_DETAILS = "ENTER_ALL_DETAILS";
        public const string CREDIT_NOTE_SUCCESS = "CREDIT_NOTE_SUCCESS";
        public const string AUTHORISATION_TRANSACTION_TYPE = "AUTHORISATION_TRANSACTION_TYPE";
        public const string SUPPLIER_ORGANISATIONS = "SUPPLIER_ORGANISATIONS";
        public const string REFERRER_NAME = "REFERRER_NAME";
        public const string SIGN_UP = "SIGN_UP";
        public const string REGISTERED = "REGISTERED";
        public const string DETAILS_CHECK = "DETAILS_CHECK";
        public const string PROFILE_CHECK = "PROFILE_CHECK";
        public const string EDIT_PRIMARY_CONTACT_DETAILS = "EDIT_PRIMARY_CONTACT_DETAILS";
        public const string PRIMARY_EMAIL = "PRIMARY_EMAIL";
        public const string PRIMARY_FIRST_NAME = "PRIMARY_FIRST_NAME";
        public const string PRIMARY_LAST_NAME = "PRIMARY_LAST_NAME";
        public const string TELEPHONE = "TELEPHONE";
        // public const string JOB_TITLE = "JOB_TITLE";
        public const string ADD_REFERRER = "ADD_REFERRER";
        public const string PRIMARY_REFERRER = "PRIMARY_REFERRER";
        public const string BACK_TO_SUPPLIER_ORGANISATIONS = "BACK_TO_SUPPLIER_ORGANISATIONS";
        // public const string GENERAL_INFORMATION = "GENERAL_INFORMATION";
        //public const string YES = "YES";
        // public const string NO = "NO";
        public const string VAT_NO_VALIDATION = "VAT_NO_VALIDATION";
        public const string DUNS_NO_VALIDATION = "DUNS_NO_VALIDATION";
        // public const string SUPPLIER_ADDRESS = "SUPPLIER_ADDRESS";
        public const string ADD_OR_EDIT_REFERRER = "ADD_OR_EDIT_REFERRER";
        public const string EDIT_PROFILE = "EDIT_PROFILE";
        public const string REFERRER_NAVIGATE_VALIDATION = "REFERRER_NAVIGATE_VALIDATION";
        public const string SELECT_INDUSTRY_SECTOR = "SELECT_INDUSTRY_SECTOR";
        public const string AUDIT_QUESTIONS_VALIDATION = "AUDIT_QUESTIONS_VALIDATION";
        public const string VAT_NUMBER_VALIDATION = "VAT_NUMBER_VALIDATION";
        public const string REFERRER_VALIDATION = "REFERRER_VALIDATION";
        public const string VERIFICATION_STATUS = "VERIFICATION_STATUS";
        public const string REASON_CODE_SUB_STATUS = "REASON_CODE_SUB_STATUS";
        public const string COMMENTS = "COMMENTS";
        public const string COMMENTS_VISIBLE_TO = "COMMENTS_VISIBLE_TO";
        public const string PRIMARY_EMAIL_ADDRESS_VALIDATION = "PRIMARY_EMAIL_ADDRESS_VALIDATION";
        public const string VALID_PRIMARY_EMAIL = "VALID_PRIMARY_EMAIL";
        public const string PRIMARY_FIRST_NAME_VALIDATION = "PRIMARY_FIRST_NAME_VALIDATION";
        public const string PRIMARY_LAST_NAME_VALIDATION = "PRIMARY_LAST_NAME_VALIDATION";
        public const string TELEPHONE_NUMBER_VALIDATION = "TELEPHONE_NUMBER_VALIDATION";
        public const string VALID_TELEPHONE_NUMBER = "VALID_TELEPHONE_NUMBER";
        public const string SEARCH_FOR_SUPPLIER_ORG = "SEARCH_FOR_SUPPLIER_ORG";
        public const string SEARCH_FOR_SUPPLIER_ID = "SEARCH_FOR_SUPPLIER_ID";
        public const string CHECKED_LOWER_CASE = "CHECKED_LOWER_CASE";

        public const string GET_SUPPLIER_INFORMATION = "GET_SUPPLIER_INFORMATION";
        public const string SUPPLIER_INFORMATION = "SUPPLIER_INFORMATION";
        //public const string SUPPLIER_DETAILS = "SUPPLIER_DETAILS";
        public const string PARENT = "PARENT";
        public const string SUBSIDIARY = "SUBSIDIARY";
        public const string PARENT_NAME_VALIDATION = "PARENT_NAME_VALIDATION";
        public const string PARENT_DUNS_NUMBER_VALIDATION = "PARENT_DUNS_NUMBER_VALIDATION";
        public const string ADDRESS_VALIDATION = "ADDRESS_VALIDATION";
        public const string CITY_VALIDATION = "CITY_VALIDATION";
        public const string POSTAL_CODE_VALIDATION = "POSTAL_CODE_VALIDATION";
        public const string COUNTRY_VALIDATION = "COUNTRY_VALIDATION";
        //public const string SALES_CONTACT_PRIMARY = "SALES_CONTACT_PRIMARY";
        public const string ACC_FIN_REMIT_CONTACT_INFO = "ACC_FIN_REMIT_CONTACT_INFO";
        public const string EMAIL_VALIDATION = "EMAIL_VALIDATION";
        public const string JOB_TITLE_VALIDATION = "JOB_TITLE_VALIDATION";
        public const string SUSTAINABILITY_CONTACT_INFO = "SUSTAINABILITY_CONTACT_INFO";
        public const string PROCUREMENT_CONTACT_INFO = "PROCUREMENT_CONTACT_INFO";
        public const string HS_CONTACT_INFO = "HS_CONTACT_INFO";
        public const string SIZE_AND_SECTOR = "SIZE_AND_SECTOR";
        public const string CUSTOMER_SECTOR_VALIDATION = "CUSTOMER_SECTOR_VALIDATION";
        //public const string SELECT_SIC_CODE = "SELECT_SIC_CODE";
        // public const string EDIT = "EDIT";
        public const string GEOGRAPHIC_COVERAGE = "GEOGRAPHIC_COVERAGE";
        public const string GEOGRAPHIC_VALIDATION = "GEOGRAPHIC_VALIDATION";
        public const string CSI_VERIFICATION = "CSI_VERIFICATION";
        // public const string VAT_NUMBER = "VAT_NUMBER";
        public const string VAT_FOR_CSI_VERIFICATION = "VAT_FOR_CSI_VERIFICATION";
        public const string VAT_COUNTRY_FOR_CSI_VERIFICATION = "VAT_COUNTRY_FOR_CSI_VERIFICATION";
        public const string CSI_VERIFICATION_COMMENTS = "CSI_VERIFICATION_COMMENTS";
        public const string COMPLETE_VERIFICATION = "COMPLETE_VERIFICATION";
        public const string FILE_EXTENSIONS_VALIDATION = "FILE_EXTENSIONS_VALIDATION";
        public const string BACK_TO = "BACK_TO";
        public const string DASHBOARD = "DASHBOARD";
        public const string BUSINESS_DESC_CHAR_LIMIT = "BUSINESS_DESC_CHAR_LIMIT";
        public const string BUSINESS_DESC_VALIDATION = "BUSINESS_DESC_VALIDATION";
        public const string ENTER_FIRST_NAME = "ENTER_FIRST_NAME";
        public const string SURNAME = "SURNAME";
        public const string SELECT_STARTED_YEAR = "SELECT_STARTED_YEAR";
        public const string UPLOAD_IMAGE_VALIDATION = "UPLOAD_IMAGE_VALIDATION";




        #endregion

        #region BUYER CONSTANTS

        public const string CONTINUE = "CONTINUE";
        public const string INBOX_BULK_REQUEST_LEAVE_POP_UP_HEADER = "INBOX_BULK_REQUEST_LEAVE_POP_UP_HEADER";
        public const string INBOX_BULK_REQUEST_LEAVE_POP_UP_MESSAGE = "INBOX_BULK_REQUEST_LEAVE_POP_UP_MESSAGE";

        public const string FROM_SUPPLIERS = "FROM_SUPPLIERS";
        public const string MESSAGE_FROM_SUPPLIERS = "MESSAGE_FROM_SUPPLIERS";
        public const string BREAD_CRUMB_BACK_TO_INBOX = "BREAD_CRUMB_BACK_TO_INBOX";
        public const string MESSAGE_TO_SUPPLIERS = "MESSAGE_TO_SUPPLIERS";
        public const string SUPPLIERS_REQUESTED_SUCCESFULLY = "SUPPLIERS_REQUESTED_SUCCESFULLY";
        public const string SELECT_SUPPLIERS_TO_BE_REQUESTED = "SELECT_SUPPLIERS_TO_BE_REQUESTED";
        public const string BUYER_HOME = "BUYER_HOME";
        public const string COMPLIANCE_REPORT = "COMPLIANCE_REPORT";
        public const string STATUS_OF_SUPPLIER_QUESTIONNAIRES = "STATUS_OF_SUPPLIER_QUESTIONNAIRES";
        public const string SUPPLIER_ONBOARDING = "SUPPLIER_ONBOARDING";
        public const string VERIFIED_SUPPLIERS = "VERIFIED_SUPPLIERS";
        public const string PROFILE_VERIFIED = "PROFILE_VERIFIED";
        public const string RESULTS = "RESULTS";
        public const string SEARCH_FOR_SUPPLIER = "SEARCH_FOR_SUPPLIER";
        public const string YOU_TRADE_WITH = "YOU_TRADE_WITH";
        public const string UNREAD_MESSAGES = "UNREAD_MESSAGES";
        public const string IN_YOUR = "IN_YOUR";
        public const string UNREAD_MESSAGE = "UNREAD_MESSAGE";
        public const string VERIFIED_SUPPLIER = "VERIFIED_SUPPLIER";
        public const string SUPPLIER_YOU_TRADE = "SUPPLIER_YOU_TRADE";
        public const string FAVOURITE_SUPPLIER = "FAVOURITE_SUPPLIER";
        public const string FLAGGED_LOWER_CASE = "FLAGGED_LOWER_CASE";
        public const string SELF_DECLARED_LOWER_CASE = "SELF_DECLARED_LOWER_CASE";
        public const string VERIFIED_LOWER_CASE = "VERIFIED_LOWER_CASE";
        public const string ANSWERS_LOWER_CASE = "ANSWERS_LOWER_CASE";
        public const string ANSWER_LOWER_CASE = "ANSWER_LOWER_CASE";
        public const string RESULTS_LOWER_CASE = "RESULTS_LOWER_CASE";
        public const string SUPPLIER_LOWER_CASE = "SUPPLIER_LOWER_CASE";
        public const string FLAGGED_TITLE_CASE = "FLAGGED_TITLE_CASE";
        public const string SELF_DECLARED_TITLE_CASE = "SELF_DECLARED_TITLE_CASE";
        public const string VERIFIED_TITLE_CASE = "VERIFIED_TITLE_CASE";
        public const string COMPLETE = "COMPLETE";
        public const string NOT_SUBMITTED = "NOT_SUBMITTED";
        public const string SUBMITTED = "SUBMITTED";
        public const string PAID = "PAID";
        public const string AUDITED = "AUDITED";
        public const string TOTAL_REQUIRED = "TOTAL_REQUIRED";
        public const string IN_SCOPE_LOWER_CASE = "IN_SCOPE_LOWER_CASE";
        public const string INVITED_BUT_NOT_BEGUN_REG = "INVITED_BUT_NOT_BEGUN_REG";
        public const string BEGUN_REG_BUT_NOT_SUBMITTED = "BEGUN_REG_BUT_NOT_SUBMITTED";
        public const string SUBMITTED_REG = "SUBMITTED_REG";
        public const string REG_VERIFIED = "REG_VERIFIED";
        public const string PROFILE_VERIFIED_SENTENCE_CASE = "PROFILE_VERIFIED_SENTENCE_CASE";
        public const string VIEW_SUPPLIERS = "VIEW_SUPPLIERS";
        public const string BREAD_CRUMB_BACK_TO_HOME = "BREAD_CRUMB_BACK_TO_HOME";
        public const string REGISTRATION_VERIFIED = "REGISTRATION_VERIFIED";
        public const string KEY_QUESTIONS = "KEY_QUESTIONS";
        public const string RISK_ANALYSIS = "RISK_ANALYSIS";
        public const string BRITVIC_EBP = "BRITVIC_EBP";
        public const string REGISTRATION_CHECKS = "REGISTRATION_CHECKS";
        public const string SUPPLIER_TYPE = "SUPPLIER_TYPE";
        public const string RESULT_OF_CHECK = "RESULT_OF_CHECK";
        public const string SUPPLIER_NAME = "SUPPLIER_NAME";
        public const string SUPPLIER_ID = "SUPPLIER_ID";
        public const string REGISTRATION_STARTED = "REGISTRATION_STARTED";
        public const string REGISTRATION_SUBMITTED = "REGISTRATION_SUBMITTED";
        public const string VIEW_PROFILE = "VIEW_PROFILE";
        public const string COMPLIANCE_AREA = "COMPLIANCE_AREA";
        public const string SHOW_RESULTS_AS = "SHOW_RESULTS_AS";
        public const string DUNS = "DUNS";
        public const string VAT = "VAT";
        public const string DBA = "DBA";
        public const string SECTOR = "SECTOR";
        public const string ADDRESS = "ADDRESS";
        public const string ESTABLISHED_YEAR = "ESTABLISHED_YEAR";
        public const string FIRM_INCORPORATION = "FIRM_INCORPORATION";
        public const string REGISTRATION_DATE = "REGISTRATION_DATE";
        public const string DOCUMENT = "DOCUMENT";
        public const string EBP_DATE = "EBP_DATE";
        public const string FLAGGED = "FLAGGED";
        public const string SELF_DECLARED = "SELF_DECLARED";
        public const string BREAD_CRUMB_BACK_TO_KEY_QUESTIONS = "BREAD_CRUMB_BACK_TO_KEY_QUESTIONS";
        public const string SUPPLIERS_LOWER_CASE = "SUPPLIERS_LOWER_CASE";
        public const string RESULT_LOWER_CASE = "RESULT_LOWER_CASE";
        public const string VIEW_ANSWER = "VIEW_ANSWER";
        public const string VIEW_ANSWER_IN_NEW_TAB = "VIEW_ANSWER_IN_NEW_TAB";
        public const string VIEW_PROFILE_IN_NEW_TAB = "VIEW_PROFILE_IN_NEW_TAB";
        public const string NUMBER_OF = "NUMBER_OF";
        public const string QUESTIONS = "QUESTIONS";
        public const string BREAD_CRUMB_BACK_TO_RISK_ANALYSIS = "BREAD_CRUMB_BACK_TO_RISK_ANALYSIS";
        public const string SHOW_SUPPLIERS = "SHOW_SUPPLIERS";

        public const string SUPPLIER_SEARCH = "SUPPLIER_SEARCH";
        public const string CLEAR = "CLEAR";
        public const string SUPPLIER_STATUS = "SUPPLIER_STATUS";
        public const string EMPLOYEE_SIZE = "EMPLOYEE_SIZE";
        public const string NO_OF_EMPLOYEES = "NO_OF_EMPLOYEES";
        public const string TURNOVER = "TURNOVER";
        public const string INCORPORATION_TYPE = "INCORPORATION_TYPE";
        public const string SEARCH_RESULTS = "SEARCH_RESULTS";
        public const string REQUEST_ANSWERS = "REQUEST_ANSWERS";
        public const string FAVOURITE = "FAVOURITE";
        public const string TRADING_WITH = "TRADING_WITH";
        public const string ANSWERS_SHARED = "ANSWERS_SHARED";
        public const string REGISTERED_DATE = "REGISTERED_DATE";
        public const string STARTED_DATE = "STARTED_DATE";
        public const string SUBMIT = "SUBMIT";
        public const string REQUIRED = "REQUIRED";
        public const string SUPPLIER_DISCUSSION = "SUPPLIER_DISCUSSION";
        public const string BACK_TO_SEARCH = "BACK_TO_SEARCH";
        public const string MESSAGE_TO_THE_SUPPLIERS = "MESSAGE_TO_THE_SUPPLIERS";
        public const string ARE_YOU_SURE_YOU_WANT_TO_REQUEST_ALL_ANSWERS = "ARE_YOU_SURE_YOU_WANT_TO_REQUEST_ALL_ANSWERS";
        public const string LOSE_YOUR_CURRENT_SELECTION = "LOSE_YOUR_CURRENT_SELECTION";
        public const string LEAVING_THIS_PAGE_WILL_LOSE_YOUR_CURRENT_SELECTION = "LEAVING_THIS_PAGE_WILL_LOSE_YOUR_CURRENT_SELECTION";
        public const string YOU_ARE_ABLE_TO_REQUEST_ANSWERS_FROM = "YOU_ARE_ABLE_TO_REQUEST_ANSWERS_FROM";
        public const string SUPPLIERS_PER_CALENDER_MONTH = "SUPPLIERS_PER_CALENDER_MONTH";
        public const string COMPLETING_THIS_REQUEST_WILL_LEAVE_YOU = "COMPLETING_THIS_REQUEST_WILL_LEAVE_YOU";
        public const string MORE_REQUESTS_THIS_CALENDER_MONTH = "MORE_REQUESTS_THIS_CALENDER_MONTH";
        public const string SEARCH_RESULTS_FOR = "SEARCH_RESULTS_FOR";
        public const string RESULTS_FOR = "RESULTS_FOR";
        public const string START_A_DISCUSSION = "START_A_DISCUSSION";
        public const string PLEASE_ENTER_COMMENTS = "PLEASE_ENTER_COMMENTS";
        public const string ALL_OF_THE_SELECTED_SUPPLIERS_HAVE_ALREADY_RECEIVED_YOUR_REQUEST = "ALL_OF_THE_SELECTED_SUPPLIERS_HAVE_ALREADY_RECEIVED_YOUR_REQUEST";
        public const string ACTION_ON = "ACTION_ON";
        public const string SELECTED = "SELECTED";
        public const string YOU_ALREADY_REQUESTED = "YOU_ALREADY_REQUESTED";
        public const string SUPPLIERS_THIS_CALENDER_MONTH = "SUPPLIERS_THIS_CALENDER_MONTH";
        public const string SUPPLIERS_REQUESTED_SUCCESSFULLY = "SUPPLIERS_REQUESTED_SUCCESSFULLY";
        public const string PLEASE_ADD_A_MESSAGE = "PLEASE_ADD_A_MESSAGE";
        public const string MESSAGE_SENT = "MESSAGE_SENT";
        public const string EXPORT_ONLY_AVAILABLE = "EXPORT_ONLY_AVAILABLE";
        public const string REGISTRATION_NUMBER = "REGISTRATION_NUMBER";
        public const string VAT_NUMBER = "VAT_NUMBER";
        public const string REGISTER_INTEREST = "REGISTER_INTEREST";
        public const string REGISTER_A_BUYER = "REGISTER_A_BUYER";
        public const string ABOUT_YOUR_ORGANISATION_PROFILE = "ABOUT_YOUR_ORGANISATION_PROFILE";
        public const string ABOUT_YOUR_ORGANISATION = "ABOUT_YOUR_ORGANISATION";
        public const string ABOUT_YOU = "ABOUT_YOU";
        public const string SUBMISSION_SUCCESSFUL = "SUBMISSION_SUCCESSFUL";
        public const string YOUR_DETAILS_HAVE_BEEN_SUBMITTED_FOR_VERIFICATION = "YOUR_DETAILS_HAVE_BEEN_SUBMITTED_FOR_VERIFICATION";

        #endregion

        #region Supplier CONSTANTS
        public const string FROM_BUYERS = "FROM_BUYERS";
        public const string MESSAGE_FROM_BUYERS = "MESSAGE_FROM_BUYERS";

        public const string BUYERS_YOU_TRADE = "BUYERS_YOU_TRADE";

        public const string FAVOURITE_BUYERS = "FAVOURITE_BUYERS";

        public const string ACCESS_TO_YOUR_ANSWERS = "ACCESS_TO_YOUR_ANSWERS";
        public const string SEND_APPLY = "SEND_APPLY";
        public const string SUPPLIER_BULK_SHARE_POP_UP_MESSAGE_BANNER = "SUPPLIER_BULK_SHARE_POP_UP_MESSAGE_BANNER";
        public const string SHARE_ANSWERS = "SHARE_ANSWERS";
        public const string SUPPLIER_SHARE_ALL_ANSWER_HEADER = "SUPPLIER_SHARE_ALL_ANSWER_HEADER";
        public const string SUPPLIER_SHARE_ALL_ANSWER_MESSAGE = "SUPPLIER_SHARE_ALL_ANSWER_MESSAGE";
        public const string SUPPLIER_MESSAGE_SENT_SUCCESSFULLY = "SUPPLIER_MESSAGE_SENT_SUCCESSFULLY";
        public const string PLEASE_ADD_MESSAGE = "PLEASE_ADD_MESSAGE";
        public const string SELECT_BUYERS_FOR_ACCESS = "SELECT_BUYERS_FOR_ACCESS";
        public const string ADD_MESSAGE_OR_EDIT_ANSWERS = "ADD_MESSAGE_OR_EDIT_ANSWERS";
        public const string ADD_MESSAGE_TO_ACCOMPANY_ACCESS = "ADD_MESSAGE_TO_ACCOMPANY_ACCESS";

        public const string FIRST_NAME_ERROR = "FIRST_NAME_ERROR";
        public const string LAST_NAME_ERROR = "LAST_NAME_ERROR";
        public const string EMAIL_ERROR = "EMAIL_ERROR";
        public const string INVALID_EMAIL_ADDRESS = "INVALID_EMAIL_ADDRESS";
        public const string EMAIL_ADDRESS_LENGTH = "EMAIL_ADDRESS_LENGTH";
        public const string ORGANISATION_NAME = "ORGANISATION_NAME";
        public const string ORGANISATION_NAME_ERROR = "ORGANISATION_NAME_ERROR";
        public const string ADDRESS_LINE_1 = "ADDRESS_LINE_1";
        public const string ADDRESS_LINE_2 = "ADDRESS_LINE_2";
        public const string COUNTY_STATE = "COUNTY_STATE";
        public const string COMPANY_TYPE = "COMPANY_TYPE";
        public const string CITY = "CITY";
        public const string ADDRESS_LINE_1_ERROR = "ADDRESS_LINE_1_ERROR";
        public const string COUNTY_STATE_ERROR = "COUNTY_STATE_ERROR";
        public const string COMPANY_TYPE_ERROR = "COMPANY_TYPE_ERROR";
        public const string CITY_ERROR = "CITY_ERROR";
        public const string COUNTRY_ERROR = "COUNTRY_ERROR";
        public const string POSTAL_ZIPCODE = "POSTAL_ZIPCODE";
        public const string POSTAL_ZIPCODE_ERROR = "POSTAL_ZIPCODE_ERROR";
        public const string NUMBER_OF_EMPLOYEES = "NUMBER_OF_EMPLOYEES";
        public const string NUMBER_OF_EMPLOYEES_ERROR = "NUMBER_OF_EMPLOYEES_ERROR";
        public const string TURNOVER_MESSAGE = "TURNOVER_MESSAGE";
        public const string TURNOVER_ERROR = "TURNOVER_ERROR";
        public const string SECTOR_DESCRIPTION_MESSAGE = "SECTOR_DESCRIPTION_MESSAGE";
        public const string SECTOR_DESCRIPTION_ERROR = "SECTOR_DESCRIPTION_ERROR";
        public const string CUSTOMER_SECTOR_LIST = "CUSTOMER_SECTOR_LIST";
        public const string GEOGRAPHIC_SALES_LIST = "GEOGRAPHIC_SALES_LIST";
        public const string GEOGRAPHIC_SUPPLIERS_LIST = "GEOGRAPHIC_SUPPLIERS_LIST";
        public const string BUSINESS_DESCRIPTION = "BUSINESS_DESCRIPTION";
        public const string REM_ADDRESS_DIFFERENT = "REM_ADDRESS_DIFFERENT";
        public const string HQ_ADDRESS_DIFFERENT = "HQ_ADDRESS_DIFFERENT";
        public const string WEBSITE_LINK = "WEBSITE_LINK";
        public const string WEBSITE_LINK_ERROR = "WEBSITE_LINK_ERROR";
        public const string REG_ADDRESS_DIFFERENT = "REG_ADDRESS_DIFFERENT";
        public const string HAVE_DUNS = "HAVE_DUNS";
        public const string DUNS_NUMBER = "DUNS_NUMBER";
        public const string IS_VAT = "IS_VAT";
        public const string VAT_NUMBER_MESSAGE = "VAT_NUMBER";
        public const string LOGO = "LOGO";
        public const string TYPE_OF_COMPANY = "TYPE_OF_COMPANY";
        public const string TYPE_OF_COMPANY_ERROR = "TYPE_OF_COMPANY_ERROR";
        public const string PASSWORD_MESSAGE = "PASSWORD_MESSAGE";
        public const string CONFIRM_PASSWORD_MESSAGE = "CONFIRM_PASSWORD_MESSAGE";
        public const string PASSWORD_ERROR = "PASSWORD_ERROR";
        public const string CONFIRM_PASSWORD_ERROR = "CONFIRM_PASSWORD_ERROR";
        public const string INVALID_PASSWORD_FORMAT = "INVALID_PASSWORD_FORMAT";
        public const string PASSWORD_LENGTH = "PASSWORD_LENGTH";
        public const string COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL = "COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL";
        public const string IS_EMPLOYEE_OF_COMPANY = "IS_EMPLOYEE_OF_COMPANY";
        public const string IS_AGREE_ON_TERMS = "IS_AGREE_ON_TERMS";
        public const string IS_SUBSIDIARY_STATUS = "IS_SUBSIDIARY_STATUS";
        public const string ULTIMATE_PARENT = "ULTIMATE_PARENT";
        public const string PARENTS_DUNS_NUMBER = "PARENTS_DUNS_NUMBER";
        public const string COMPANY_REG_NUMBER = "COMPANY_REG_NUMBER";
        public const string W9W8_FORM = "W9W8_FORM";
        public const string COMPANY_YEAR = "COMPANY_YEAR";
        public const string COMPANY_YEAR_ERROR = "COMPANY_YEAR_ERROR";
        public const string FIRM_STATUS = "FIRM_STATUS";
        public const string STATES_SELECTED = "STATES_SELECTED";
        public const string TRADING = "TRADING";
        public const string IS_SUBSIDIARY_STATUS_ERROR = "IS_SUBSIDIARY_STATUS_ERROR";
        public const string TEL_NUMBER = "TEL_NUMBER";
        public const string INVALID_TEL_NUMBER = "INVALID_TEL_NUMBER";
        public const string TEL_NUMBER_LENGTH = "TEL_NUMBER_LENGTH";
        public const string TEL_NUMBER_ERROR = "TEL_NUMBER_ERROR";
        public const string JOB_TITLE = "JOB_TITLE";
        public const string ORG_FACEBOOK_ACCOUNT = "ORG_FACEBOOK_ACCOUNT";
        public const string ORG_TWITTER_ACCOUNT = "ORG_TWITTER_ACCOUNT";
        public const string ORG_LINKEDIN_ACCOUNT = "ORG_LINKEDIN_ACCOUNT";
        public const string MAX_CONTRACT_VALUE = "MAX_CONTRACT_VALUE";
        public const string MAX_CONTRACT_VALUE_ERROR = "MAX_CONTRACT_VALUE_ERROR";
        public const string MIN_CONTRACT_VALUE = "MIN_CONTRACT_VALUE";
        public const string MIN_CONTRACT_VALUE_ERROR = "MIN_CONTRACT_VALUE_ERROR";
        public const string VERIFICATION_COMMENTS = "VERIFICATION_COMMENTS";
        public const string VAT_COUNTRY = "VAT_COUNTRY";
        public const string IS_VAT_REGISTERED = "IS_VAT_REGISTERED";
        public const string REG_A_SUPPLIER = "REG_A_SUPPLIER";
        public const string CREATE_AN_ACCOUNT = "CREATE_AN_ACCOUNT";
        public const string SUBMIT_REGISTRATION = "SUBMIT_REGISTRATION";
        public const string GENERAL_INFORMATION = "GENERAL_INFORMATION";
        public const string STEP_1_OF_4 = "STEP_1_OF_4";
        public const string STEP_2_OF_4 = "STEP_2_OF_4";
        public const string STEP_3_OF_4 = "STEP_3_OF_4";
        public const string STEP_4_OF_4 = "STEP_4_OF_4";
        public const string NEED_HELP_CONTACT_US = "NEED_HELP_CONTACT_US";
        public const string READ_OUR_FAQS = "READ_OUR_FAQS";
        public const string VAT_NUMBER_ERROR = "VAT_NUMBER_ERROR";
        public const string ADDRESSES = "ADDRESSES";
        public const string CAPABILITIES = "CAPABILITIES";
        public const string SELECT_AT_LEAST_ONE_CATEGORY = "SELECT_AT_LEAST_ONE_CATEGORY";
        public const string SELECT_SIC_CODE = "SELECT_SIC_CODE";
        public const string SELECT_AT_LEAST_ONE_GEOGRAPHIC_REGION = "SELECT_AT_LEAST_ONE_GEOGRAPHIC_REGION";
        public const string STATES_ERROR = "STATES_ERROR";
        public const string MARKETING_PROFILE = "MARKETING_PROFILE";
        public const string SALES_CONTACT_PRIMARY = "SALES_CONTACT_PRIMARY";
        public const string ACCOUNTS_FINANCE_REMITTANCE_CONTACT = "ACCOUNTS_FINANCE_REMITTANCE_CONTACT";
        public const string SUSTAINABILITY_CONTACT = "SUSTAINABILITY_CONTACT";
        public const string HS_CONTACT = "HS_CONTACT";
        public const string PROCUREMENT_CONTACT = "PROCUREMENT_CONTACT";
        public const string REGISTER = "REGISTER";
        public const string CONTACTS = "CONTACTS";
        public const string NEXT = "NEXT";
        public const string SUPPLIER_DETAILS = "SUPPLIER_DETAILS";
        public const string NO = "NO";
        public const string YES = "YES";
        public const string EDIT = "EDIT";
        public const string SUPPLIER_ADDRESS = "SUPPLIER_ADDRESS";
        public const string ADDRESS_ERROR = "ADDRESS_ERROR";
        public const string JOB_TITLE_ERROR = "JOB_TITLE_ERROR";
        public const string PREVIOUS = "PREVIOUS";

        #endregion

        public const string SUBMITTED_AND_BEING_AUDITED = "SUBMITTED_AND_BEING_AUDITED";
        public const string FLAGGED_ANSWERS = "FLAGGED_ANSWERS";
        public const string SUPPLIER_HOME_BANNER_1 = "SUPPLIER_HOME_BANNER_1";
        public const string SUPPLIER_HOME_BANNER_2 = "SUPPLIER_HOME_BANNER_2";
        public const string GENERAL_INFORMATION_AND_CONTACTS = "GENERAL_INFORMATION_AND_CONTACTS";
        public const string NO_REQUIRED_SECTIONS_NEEDING_ACTION = "NO_REQUIRED_SECTIONS_NEEDING_ACTION";
        public const string GENERAL_INFORMATION_AND_CONTACTS_MESSAGE = "GENERAL_INFORMATION_AND_CONTACTS_MESSAGE";
        public const string COMPLIANCE_CHECKS_MESSAGE = "COMPLIANCE_CHECKS_MESSAGE";
        public const string REQUIRED_SECTION_NEEDING_ACTION = "REQUIRED_SECTION_NEEDING_ACTION";
        public const string REQUIRED_SECTIONS_NEEDING_ACTION = "REQUIRED_SECTIONS_NEEDING_ACTION";
        public const string CIPS_SUSTAINABILITY_INDEX = "CIPS_SUSTAINABILITY_INDEX";
        public const string CIPS_SUSTAINABILITY_INDEX_MESSAGE = "CIPS_SUSTAINABILITY_INDEX_MESSAGE";
        public const string LINK_TO_CIPS_SUSTAINABILITY_INDEX = "LINK_TO_CIPS_SUSTAINABILITY_INDEX";
        public const string VIEW_YOUR_PROFILE = "VIEW_YOUR_PROFILE";
        public const string QUESTIONNAIRE_NAME = "QUESTIONNAIRE_NAME";
        public const string GO_TO_QUESTIONNAIRE = "GO_TO_QUESTIONNAIRE";
        public const string SECTION_NAME = "SECTION_NAME";
        public const string BACK_TO_HOME = "BACK_TO_HOME";
        public const string PASSWORD_CHANGED_SUCCESSFULLY = "PASSWORD_CHANGED_SUCCESSFULLY";
        public const string QUESTION = "QUESTION";
        public const string QUESTIONS_RELATING_TO_AND_ONLY_VIEWED_BY = "QUESTIONS_RELATING_TO_AND_ONLY_VIEWED_BY";
        public const string ACCEPT_ETHICAL_BUSINESS_POLICY_HERE = "ACCEPT_ETHICAL_BUSINESS_POLICY_HERE";
        public const string ANSWERED = "ANSWERED";
        public const string FLAGGED_ANSWER = "FLAGGED_ANSWER";
        public const string SECTION = "SECTION";
        public const string REQUIRED_SECTION = "REQUIRED_SECTION";
        public const string GENERAL_INFO_AND_CONTACTS = "GENERAL_INFO_AND_CONTACTS";
        public const string QUESTIONNAIRE_PENDING = "QUESTIONNAIRE_PENDING";
        public const string QUESTIONNAIRE_COMPLETE = "QUESTIONNAIRE_COMPLETE";
        public const string EMPLOYEE_OF_COMPANY = "EMPLOYEE_OF_COMPANY";
        public const string SUPPLIER_HOME = "SUPPLIER_HOME";
        public const string SUPPLIER_HOME_BANNER = "SUPPLIER_HOME_BANNER";
        public const string REQUIRED_SECTIONS = "REQUIRED_SECTIONS";
        public const string NEEDING_ACTION = "NEEDING_ACTION";
        public const string BUT = "BUT";
        public const string I_HAVE_READ_AND_AGREE_TO_THE = "I_HAVE_READ_AND_AGREE_TO_THE";
        public const string TERMS_AND_CONDITIONS = "TERMS_AND_CONDITIONS";
        public const string DUNS_NUMBER_ERROR = "DUNS_NUMBER_ERROR";
        public const string ULTIMATE_PARENT_ERROR = "ULTIMATE_PARENT_ERROR";
        public const string BACK = "BACK";
        public const string QUESTIONNAIRE = "QUESTIONNAIRE";
        public const string PASSWORD_SHOULD_CONTAIN_ATLEAST = "PASSWORD_SHOULD_CONTAIN_ATLEAST";
        public const string OLD_PASSWORD_MESSAGE = "OLD_PASSWORD_MESSAGE";
        public const string OLD_PASSWORD_ERROR = "OLD_PASSWORD_ERROR";
        public const string NEW_PASSWORD_MESSAGE = "NEW_PASSWORD_MESSAGE";
        public const string NEW_PASSWORD_ERROR = "NEW_PASSWORD_ERROR";
        public const string PASSWORD_RESET_FAIL = "PASSWORD_RESET_FAIL";
        public const string PASSWORD_GUIDELINES = "PASSWORD_GUIDELINES";
        public const string ENTER_CORRECT_PASSWORD = "ENTER_CORRECT_PASSWORD";
        public const string NEW_PASSWORD_SENT_TO_MAIL = "NEW_PASSWORD_SENT_TO_MAIL";
        public const string SUPPLIER_PORTAL = "SUPPLIER_PORTAL";
        public const string WHY_REGISTER = "WHY_REGISTER";
        public const string TO_THE = "TO_THE";
        public const string WELCOME_TO_OUR_NEW_SUPPLIER_PORTAL = "WELCOME_TO_OUR_NEW_SUPPLIER_PORTAL";
        public const string VALID = "VALID";
        public const string INVALID = "INVALID";
        public const string I_HAVE_READ_AND_AGREE_TO_THE_TERMS_AND_CONDITIONS = "I_HAVE_READ_AND_AGREE_TO_THE_TERMS_AND_CONDITIONS";
        public const string PARENT_NAME_ERROR = "PARENT_NAME_ERROR";
        public const string PARENT_DUNS_NUMBER_ERROR = "PARENT_DUNS_NUMBER_ERROR";
        public const string INDUSTRY_SECTOR_ERROR = "INDUSTRY_SECTOR_ERROR";
        public const string ADD = "ADD";
        public const string OK = "OK";
        public const string COUNTRY_OR_STATE = "COUNTRY_OR_STATE";
        public const string SELECT_SECTIONS_TO_PAY_FOR = "SELECT_SECTIONS_TO_PAY_FOR";
        public const string ENTER_PAYMENT_INFORMATION = "ENTER_PAYMENT_INFORMATION";
        public const string DOWNLOAD_RECEIPT_OR_INVOICE = "DOWNLOAD_RECEIPT_OR_INVOICE";
        public const string ANSWERS_SUBMITTED_AND_AWAITING_PAYMENT = "ANSWERS_SUBMITTED_AND_AWAITING_PAYMENT";
        public const string VOUCHER_CODE = "VOUCHER_CODE";
        public const string APPLY_VOUCHER = "APPLY_VOUCHER";
        public const string PAYMENT_TYPE = "PAYMENT_TYPE";
        public const string PAYMENT_AMMOUNT_NET = "PAYMENT_AMMOUNT_NET";
        public const string VOUCHER_DISCOUNT = "VOUCHER_DISCOUNT";
        public const string SUB_TOTAL = "SUB_TOTAL";
        public const string ORGANISATION_NAME_MESSAGE = "ORGANISATION_NAME_MESSAGE";
        public const string ORGANISATION_NAME_MESSAGE_ERROR = "ORGANISATION_NAME_MESSAGE_ERROR";
        public const string PAY_BY_INVOICE = "PAY_BY_INVOICE";
        public const string VOUCHER_IS_SUBJECT_TO = "VOUCHER_IS_SUBJECT_TO";
        public const string STANDARD_TERMS_AND_CONDITIONS = "STANDARD_TERMS_AND_CONDITIONS";
        public const string PRGX_CARD_PAYMENT_PROCESS_PART1 = "PRGX_CARD_PAYMENT_PROCESS_PART1";
        public const string PRGX_CARD_PAYMENT_PROCESS_PART2 = "PRGX_CARD_PAYMENT_PROCESS_PART2";
        public const string CONFIRM_BILLING_DETAILS = "CONFIRM_BILLING_DETAILS";
        public const string FAO = "FAO";
        public const string FAO_ERROR = "FAO_ERROR";
        public const string COMPLETE_TRANSACTION = "COMPLETE_TRANSACTION";
        public const string CARD = "CARD";
        public const string NO_SECTIONS_AWAITING_PAYMENT = "NO_SECTIONS_AWAITING_PAYMENT";
        public const string VOUCHER_ERROR = "VOUCHER_ERROR";
        public const string PLEASE_ENTER_A_VOUCHER_CODE = "PLEASE_ENTER_A_VOUCHER_CODE";
        public const string REMOVE_VOUCHER = "REMOVE_VOUCHER";
        public const string SELECT_ATLEAST_ONE_PRODUCT = "SELECT_ATLEAST_ONE_PRODUCT";
        public const string PAYMENT_TYPE_AS_INVOICE = "PAYMENT_TYPE_AS_INVOICE";
        public const string CREDIT_NO = "CREDIT_NO";
        public const string DATE = "DATE";
        public const string INVOICE_REF = "INVOICE_REF";
        public const string PAYMENT_VIA = "PAYMENT_VIA";
        public const string REFUND_DESCRIPTION = "REFUND_DESCRIPTION";
        public const string PRGX_UK_LIMITED = "PRGX_UK_LIMITED";
        public const string COMPANY_REGISTRATION_NUMBER = "COMPANY_REGISTRATION_NUMBER";
        public const string TEL = "TEL";
        public const string VALUE = "VALUE";
        public const string PAYABLE_TO = "PAYABLE_TO";
        public const string ONE_UPPER_CASE_LETTER = "ONE_UPPER_CASE_LETTER";
        public const string ONE_LOWER_CASE_LETTER = "ONE_LOWER_CASE_LETTER";
        public const string ONE_NUMBER = "ONE_NUMBER";
        public const string ONE_SPECIAL_CHARACTER = "ONE_SPECIAL_CHARACTER";
        public const string PASSWORD_SHOULD_NOT_START_OR_END_WITH_SPACES = "PASSWORD_SHOULD_NOT_START_OR_END_WITH_SPACES";
        public const string ORGANISATION_NAME_BUYER = "ORGANISATION_NAME_BUYER";
        public const string ABOUT_BUYER = "ABOUT_BUYER";
        public const string ABOUT_BUYER_ORGANISATION = "ABOUT_BUYER_ORGANISATION";
        public const string ABOUT_BUYER_BUSINESS_PROFILE = "ABOUT_BUYER_BUSINESS_PROFILE";
        public const string QUESTION_SET_DETAILS = "QUESTION_SET_DETAILS";
        public const string BACK_TO_MANAGE_QUESTIONNAIRES = "BACK_TO_MANAGE_QUESTIONNAIRES";
        public const string BACK_TO_ALL_QUESTIONNAIRE_SECTIONS = "BACK_TO_ALL_QUESTIONNAIRE_SECTIONS";
        public const string SEARCH_FOR_QUESTION = "SEARCH_FOR_QUESTION";
        public const string INDEX = "INDEX";
        public const string LEGAL_AND_SANCTION_VERIFICATION = "LEGAL_AND_SANCTION_VERIFICATION";
        public const string SANCTION_AND_LEGAL_QUESTIONNAIRE = "SANCTION_AND_LEGAL_QUESTIONNAIRE";
        public const string RISK_COUNTRY_ALERTS = "RISK_COUNTRY_ALERTS";
        public const string PERSON_SANCTION_ENTITY_LISTS = "PERSON_SANCTION_ENTITY_LISTS";
        public const string PLEASE_AUDIT_ALL_QUESTIONS_TO_SAVE = "PLEASE_AUDIT_ALL_QUESTIONS_TO_SAVE";
        public const string SANCTION_ANSWERS_SAVED_SUCCESSFULLY = "SANCTION_ANSWERS_SAVED_SUCCESSFULLY";
        public const string COMPANY_VERIFICATION_COMPLETE = "COMPANY_VERIFICATION_COMPLETE";
        public const string SUB_STATUS = "SUB_STATUS";
        public const string CREATE_VOUCHER = "CREATE_VOUCHER";
        public const string ERROR_WHILE_INSERTING = "ERROR_WHILE_INSERTING";
        public const string LATEST_TERMS_VERSION = "LATEST_TERMS_VERSION";
        public const string ORIGINAL_TERMS_VERSION = "ORIGINAL_TERMS_VERSION";
        public const string LATEST_TERMS_ACCEPTED_DATE = "LATEST_TERMS_ACCEPTED_DATE";
        public const string LAST_LOGIN_DATE = "LAST_LOGIN_DATE";
        public const string USER_ADDED_SUCCESSFULLY = "USER_ADDED_SUCCESSFULLY";
        public const string USER_UPDATED_SUCCESSFULLY = "USER_UPDATED_SUCCESSFULLY";

        #region Campaign
        public const string MASTER_VENDOR = "MASTER_VENDOR";
        public const string CAMPAIGN_URL = "CAMPAIGN_URL";
        public const string WELCOME_MESSAGE = "WELCOME_MESSAGE";
        public const string NOTE = "NOTE";
        public const string PAGE_TEMPLATE = "PAGE_TEMPLATE";
        public const string EMAIL_CONTENT = "EMAIL_CONTENT";
        public const string PREREGISTRATION_FILE = "PREREGISTRATION_FILE";
        public const string DATA_SOURCE = "DATA_SOURCE";
        public const string SUPPLIERS_IN_CAMPAIGN = "SUPPLIERS_IN_CAMPAIGN";
        public const string SELECTION_CRITERIA = "SELECTION_CRITERIA";
        public const string INVALID_COMMENTS = "INVALID_COMMENTS";
        public const string SUBMITTING_OF_CAMPAIGNS_WOULD_DELETE_ALL_THE_INVALID_ENTRIES = "SUBMITTING_OF_CAMPAIGNS_WOULD_DELETE_ALL_THE_INVALID_ENTRIES";
        public const string PRE_REG_SUPPLIER_DELETED_SUCCESSFULLY = "PRE_REG_SUPPLIER_DELETED_SUCCESSFULLY";
        public const string UNABLE_TO_DELETE_PRE_REG_SUPPLIER = "UNABLE_TO_DELETE_PRE_REG_SUPPLIER";
        public const string UNABLE_TO_SUBMIT_CAMPAIGN = "UNABLE_TO_SUBMIT_CAMPAIGN";
        public const string CAMPAIGN_SUBMITTED_SUCCESSFULLY = "CAMPAIGN_SUBMITTED_SUCCESSFULLY";
        public const string PLEASE_SELECT_A_FILE_TO_UPLOAD = "PLEASE_SELECT_A_FILE_TO_UPLOAD";
        public const string VERIFY_CAMPAIGN = "VERIFY_CAMPAIGN";
        public const string VERIFY_CAMPAIGN_LIST = "VERIFY_CAMPAIGN_LIST";
        public const string UPLOAD = "UPLOAD";
        public const string SUBMIT_CAMPAIGN = "SUBMIT_CAMPAIGN";
        public const string WELCOME = "WELCOME";
        public const string ALREADY_REGISTERED_LOGIN_HERE = "ALREADY_REGISTERED_LOGIN_HERE";
        public const string CAMPAIGN_ASSIGNED_TO_AUDITOR_SUCCESSFULLY = "CAMPAIGN_ASSIGNED_TO_AUDITOR_SUCCESSFULLY";
        public const string CAMPAIGN_COULD_NOT_BE_ASSIGNED_TO_AUDITOR = "CAMPAIGN_COULD_NOT_BE_ASSIGNED_TO_AUDITOR";
        public const string CAMPAIGN_VERIFY_FILE_VALIDATION = "CAMPAIGN_VERIFY_FILE_VALIDATION";
        public const string SUPPLIER_COMPANY_NAME = "SUPPLIER_COMPANY_NAME";
        public const string DUNS_NUMBER_STRING = "DUNS_NUMBER_STRING";
        public const string LANDING_PAGE_URL = "LANDING_PAGE_URL";
        public const string REGISTRATION_CODE = "REGISTRATION_CODE";
        public const string CAMPAIGN_ALREADY_REGISTERED_MESSAGE = "CAMPAIGN_ALREADY_REGISTERED_MESSAGE";
        public const string CAMPAIGN_INCORRECT_REGISTRATION_CODE = "CAMPAIGN_INCORRECT_REGISTRATION_CODE";
        #endregion


        public const string ACCIDENT_INCIDENT_TYPE = "ACCIDENT_INCIDENT_TYPE";
        public const string ACCOUNT = "ACCOUNT";
        public const string ADD_ANOTHER_ANSWER_TYPE = "ADD_ANOTHER_ANSWER_TYPE";
        public const string ADD_ANOTHER_OPTION = "ADD_ANOTHER_OPTION";
        public const string ADD_BUYER = "ADD_BUYER";
        public const string ADD_USER_SUCCESS = "ADD_USER_SUCCESS";
public const string AGGREGATE_COVERAGE = "AGGREGATE_COVERAGE";
        public const string ALL_PRODUCTS_MAPPED_SUCCESSFULLY = "ALL_PRODUCTS_MAPPED_SUCCESSFULLY";

        public const string ANSWER_SECTION_COMPLETELY = "ANSWER_SECTION_COMPLETELY";
        public const string ANSWER_TEXT = "ANSWER_TEXT";
        public const string ANSWER_TYPE = "ANSWER_TYPE";
        public const string ANSWER_TYPE_ERROR = "ANSWER_TYPE_ERROR";
        public const string ANSWER_TYPE_OPTION_ERROR = "ANSWER_TYPE_OPTION_ERROR";
        public const string ANSWER_TYPE_VALIDATION = "ANSWER_TYPE_VALIDATION";

        public const string ANSWERS_SUBMITTED_BUT_AWAITING_PAYMENT = "ANSWERS_SUBMITTED_BUT_AWAITING_PAYMENT";
        public const string ASSIGN_BANK_DETAILS = "ASSIGN_BANK_DETAILS";
        public const string ASSIGN_CLIENTS = "ASSIGN_CLIENTS";
        public const string ASSIGN_CONTACTS = "ASSIGN_CONTACTS";
        public const string ASSIGN_UNASSIGN = "ASSIGN_UNASSIGN";
        public const string BACK_TO_PROFILE = "BACK_TO_PROFILE";
        public const string BACK_TO_QUESTIONNAIRE_DETAILS = "BACK_TO_QUESTIONNAIRE_DETAILS";
        public const string BANK_DETAILS_ERROR = "BANK_DETAILS_ERROR";
        public const string BROWSE_ANSWERS = "BROWSE_ANSWERS";
        public const string BUSINESS_REGISTERED = "BUSINESS_REGISTERED";
        public const string BUSINESS_VERIFIED = "BUSINESS_VERIFIED";
        public const string CAMPAIGNS = "CAMPAIGNS";
public const string CERTIFICATE_NUMBER = "CERTIFICATE_NUMBER";
        public const string CERTIFYING_AUTHORITY_NAME = "CERTIFYING_AUTHORITY_NAME";
        public const string CHANGE_THIS_LINE = "CHANGE_THIS_LINE";
        public const string CHECKBOX_VALIDATION = "CHECKBOX_VALIDATION";
        public const string CHECKBOXES = "CHECKBOXES";
        public const string CHOOSE_CONTROL = "CHOOSE_CONTROL";
        public const string CLIENT = "CLIENT";
        public const string CLIENT_CHECKS = "CLIENT_CHECKS";
        public const string CLIENT_COMPLIANCE = "CLIENT_COMPLIANCE";
        public const string CLIENT_COMPLIANCE_ALREADY_SUBMITTED = "CLIENT_COMPLIANCE_ALREADY_SUBMITTED";
        public const string CLIENT_DETAILS_ERROR = "CLIENT_DETAILS_ERROR";
        public const string CLIENT_QUESTIONNAIRE = "CLIENT_QUESTIONNAIRE";
        public const string COMPANY_SPECIFIC_QUESTIONS = "COMPANY_SPECIFIC_QUESTIONS";
        public const string CONTACT_DETAILS_ERROR = "CONTACT_DETAILS_ERROR";
        public const string COVERAGE_PER_OCCURENCE = "COVERAGE_PER_OCCURENCE";
        public const string CREATE_NEW_CAMPAIGN = "CREATE_NEW_CAMPAIGN";
public const string CREATE_NEW_VOUCHER = "CREATE_NEW_VOUCHER";
public const string CREATE_QUESTION = "CREATE_QUESTION";
        public const string DELETED_FROM_CSQ1 = "DELETED_FROM_CSQ1";
        public const string DELETED_FROM_CSQ2 = "DELETED_FROM_CSQ2";
        public const string DELETED_FROM_CSQ3 = "DELETED_FROM_CSQ3";
        public const string DOCUMENT_UPLOAD_VALIDATION = "DOCUMENT_UPLOAD_VALIDATION";
public const string DOWNLOAD_INVOICE_FROM = "DOWNLOAD_INVOICE_FROM";
        public const string DOWNLOAD_RECEIPT = "DOWNLOAD_RECEIPT";
        public const string DOWNLOAD_RECEIPT_FROM = "DOWNLOAD_RECEIPT_FROM";
        public const string DOWNLOAD_REPORTS = "DOWNLOAD_REPORTS";
        public const string DROPDOWN = "DROPDOWN";
        public const string DROPDOWN_VALIDATION = "DROPDOWN_VALIDATION";
        public const string EDIT_BUYER_DETAILS = "EDIT_BUYER_DETAILS";
public const string EDIT_CLIENT_DETAILS = "EDIT_CLIENT_DETAILS";
        public const string EDIT_CONTACT_DETAILS = "EDIT_CONTACT_DETAILS";
        public const string EDIT_QUESTION = "EDIT_QUESTION";
        public const string EDIT_QUESTIONS = "EDIT_QUESTIONS";
        public const string EMPLOYERS_LIABILITY = "EMPLOYERS_LIABILITY";
        public const string ENCRYPT_PASSWORD = "ENCRYPT_PASSWORD";
        public const string EVALUATE_CLIENT_QUESTIONNAIRE = "EVALUATE_CLIENT_QUESTIONNAIRE";
        public const string EXPIRY_DATE = "EXPIRY_DATE";
        public const string FACEBOOK_ACCOUNT = "FACEBOOK_ACCOUNT";
        public const string FILE = "FILE";
        public const string FILE_NAME = "FILE_NAME";
public const string FILE_UPLOAD = "FILE_UPLOAD";
        public const string FIT5_DETAILS_MESSAGE = "FIT5_DETAILS_MESSAGE";
        public const string FOUND = "FOUND";
        public const string FY = "FY";
public const string HASHED_PASSWORD = "HASHED_PASSWORD";
        public const string HQ_ADDRESS = "HQ_ADDRESS";
        public const string HS1_A_MESSAGE = "HS1_A_MESSAGE";
        public const string HS1_A_QUESTION = "HS1_A_QUESTION";
        public const string HS1_B_MESSAGE = "HS1_B_MESSAGE";
        public const string HS1_B_QUESTION = "HS1_B_QUESTION";
        public const string HS1_CASE_A_MESSAGE = "HS1_CASE_A_MESSAGE";
        public const string HS1_CASE_B1_MESSAGE = "HS1_CASE_B1_MESSAGE";
        public const string HS1_CASE_B2_MESSAGE = "HS1_CASE_B2_MESSAGE";
        public const string HS1_DEFAULT_MESSAGE = "HS1_DEFAULT_MESSAGE";
        public const string HS4_MESSAGE = "HS4_MESSAGE";
        public const string HS6_DETAILS_MESSAGE = "HS6_DETAILS_MESSAGE";
        public const string HS6_MESSAGE = "HS6_MESSAGE";
        public const string INSURANCE_COMPANY = "INSURANCE_COMPANY";
        public const string INVALID_SUPPLIER_NAMES = "INVALID_SUPPLIER_NAMES";
        public const string INVOICE_ACKNOWLEDGE = "INVOICE_ACKNOWLEDGE";
        public const string INVOICE_GENERATED = "INVOICE_GENERATED";
        public const string INVOICE_PAYMENT_MESSAGE = "INVOICE_PAYMENT_MESSAGE";
        public const string IS_FIT5_DETAILS_MESSAGE = "IS_FIT5_DETAILS_MESSAGE";
        public const string LINKEDIN_ACCOUNT = "LINKEDIN_ACCOUNT";
        public const string MAIN_INDUSTRY_SECTOR = "MAIN_INDUSTRY_SECTOR";
        public const string MANAGE_BUYERS = "MANAGE_BUYRERS";
        public const string MANAGE_LIST = "MANAGE_LIST";
public const string MAP_SECTION_TO_BUYERS = "MAP_SECTION_TO_BUYERS";
        public const string MAPPED_SUPPLIERS = "MAPPED_SUPPLIERS";
public const string MAX_CONTRACT_VALUE_DISPLAY = "MAX_CONTRACT_VALUE_DISPLAY";
        public const string MIN_CONTRACT_VALUE_DISPLAY = "MIN_CONTRACT_VALUE_DISPLAY";
        public const string MONITOR_ORDER_STATUS = "MONITOR_ORDER_STATUS";
        public const string NAME_ON_ACCOUNT = "NAME_ON_ACCOUNT";
        public const string NO_ANSWERS_TO_SHOW = "NO_ANSWERS_TO_SHOW";
        public const string NO_COVER = "NO_COVER";
        public const string NO_DBA_NAME_PROVIDED = "NO_DBA_NAME_PROVIDED";
        public const string NO_FILES_UPLOADED = "NO_FILES_UPLOADED";
        public const string NO_HQ_ADDRESS = "NO_HQ_ADDRESS";
        public const string NO_NOTICES = "NO_NOTICES";
        public const string NO_OTHER_INSURANCE_COVERAGE = "NO_OTHER_INSURANCE_COVERAGE";
        public const string NO_REGISTERED_ADDRESS = "NO_REGISTERED_ADDRESS";
        public const string NO_REMITTANCE_ADDRESS = "NO_REMITTANCE_ADDRESS";
        public const string NO_WEBSITE_ADDRESS = "NO_WEBSITE_ADDRESS";
        public const string NOT_APPLICABLE = "NOT_APPLICABLE";
        public const string NOT_AVAILABLE = "NOT_AVAILABLE";
public const string NOTICE_CONVICTION_DATE = "NOTICE_CONVICTION_DATE";
        public const string NOTICE_TYPE_CONVICTION = "NOTICE_TYPE_CONVICTION";
        public const string PAYMENT_ACKNOWLEDGE = "PAYMENT_ACKNOWLEDGE";
        public const string PAYMENT_COMPLETE = "PAYMENT_COMPLETE";
        public const string PAYMENT_CONFIRMATION = "PAYMENT_CONFIRMATION";
        public const string PAYMENT_VERIFICATION = "PAYMENT_VERIFICATION";
        public const string PLEASE_PROVIDE_DETAILS = "PLEASE_PROVIDE_DETAILS";
        public const string POLICY_ACCEPTED_ON = "POLICY_ACCEPTED_ON";
        public const string POLICY_ACCEPTED_ONLINE = "POLICY_ACCEPTED_ONLINE";
        public const string POLICY_ACCEPTED_SIGNING = "POLICY_ACCEPTED_SIGNING";
        public const string POLICY_NUMBER = "POLICY_NUMBER";
        public const string PRIMARY_ADDRESS = "PRIMARY_ADDRESS";
        public const string PRIMARY_SALES_CONTACT = "PRIMARY_SALES_CONTACT";
        public const string PUBLIC_LIABILITY = "PUBLIC_LIABILITY";
        public const string QUESTION_DISPLAY = "QUESTION_DISPLAY";
        public const string QUESTION_ERROR = "QUESTION_ERROR";
        public const string QUESTION_NUMBER = "QUESTION_NUMBER";
        public const string QUESTION_SET_ERROR = "QUESTION_SET_ERROR";
        public const string QUESTION_TEXT = "QUESTION_TEXT";
        public const string QUESTIONNAIRE_SECTION_DISPLAY = "QUESTIONNAIRE_SECTION_DISPLAY";
        public const string QUESTIONNAIRE_SECTION_ERROR = "QUESTIONNAIRE_SECTION_ERROR";
        public const string QUESTIONNAIRES = "QUESTIONNAIRES";
        public const string QUESTIONS_NOT_AVAILABLE = "QUESTIONS_NOT_AVAILABLE";
        public const string RADIO_BUTTON = "RADIO_BUTTON";
        public const string RADIO_BUTTON_VALIDATION = "RADIO_BUTTON_VALIDATION";
        public const string REASON = "REASON";
public const string REG_COUNTRY = "REG_COUNTRY";
        public const string REGISTERED_ACCOUNT = "REGISTERED_ACCOUNT";
        public const string REMITTANCE_ADDRESS = "REMITTANCE_ADDRESS";
        public const string RETURN_TO_AWAITING_PAYMENT = "RETURN_TO_AWAITING_PAYMENT";
        public const string RETURN_TO_HOMEPAGE = "RETURN_TO_HOMEPAGE";
        public const string SCOPE_OF_CERTIFICATION = "SCOPE_OF_CERTIFICATION";
        public const string SCREEN = "SCREEN";
        public const string SELECT_FILE_UPLOAD = "SELECT_FILE_UPLOAD";
        public const string SIC_CODE_MESSAGE = "SIC_CODE_MESSAGE";
        public const string SIM_TRANSACTION_NUMBER = "SIM_TRANSACTION_NUMBER";
        public const string SUBMIT_ANSWERS = "SUBMIT_ANSWERS";
        public const string SUBMIT_ANSWERS_WHEN_READY = "SUBMIT_ANSWERS_WHEN_READY";

        public const string TEXT = "TEXT";
        public const string TRANSACTION_FAILED = "TRANSACTION_FAILED";
        public const string TRANSACTION_FAILED_TRY_AGAIN = "TRANSACTION_FAILED_TRY_AGAIN";
        public const string TRANSACTION_FAILURE_MESSAGE = "TRANSACTION_FAILURE_MESSAGE";
        public const string TWITTER_ACCOUNT = "TWITTER_ACCOUNT";
        public const string TYPE_OF_INSURANCE = "TYPE_OF_INSURANCE";
        public const string UPDATE_USER_SUCCESS = "UPDATE_USER_SUCCESS";
public const string UPLOAD_FROM_FILE = "UPLOAD_FROM_FILE";
        public const string VALID_FILE_EXTENSIONS = "VALID_FILE_EXTENSIONS";
        public const string VIEW_QUESTIONNAIRE = "VIEW_QUESTIONNAIRE";
        public const string VIEW_THIS_SECTION = "VIEW_THIS_SECTION";
        public const string WEBSITE_LINK_DISPLAY = "WEBSITE_LINK_DISPLAY";

        public const string ANSWER_LAST_SUBMITTED = "ANSWER_LAST_SUBMITTED";
        public const string ADDRESS_1 = "ADDRESS_1";
        public const string ADDRESS_2 = "ADDRESS_2";
        public const string BIC_CODE = "BIC_CODE";
        public const string FINANCE_INSURANCE = "FINANCE_INSURANCE";
        public const string HEALTH = "HEALTH";
        public const string QUESTIONS_ANSWERS = "QUESTIONS_ANSWERS";
        public const string PROFILE_LOWERCASE = "PROFILE_LOWERCASE";
        public const string AUDITED_ON = "AUDITED_ON";
        public const string NOT_REQUIRED = "NOT_REQUIRED";
        public const string ACCESS_TO_ANSWERS_REQUESTED = "ACCESS_TO_ANSWERS_REQUESTED";
        public const string ALL_ANSWERS_SHARED_WITH_YOU = "ALL_ANSWERS_SHARED_WITH_YOU";
        public const string YOU_ALREADY_REQUEST = "YOU_ALREADY_REQUEST";
        public const string BROWSE_ANSWER = "BROWSE_ANSWER";
        public const string ANSWER_WILL_APPEAR_WHEN_SUBMITTED_AND_AUDITED = "ANSWER_WILL_APPEAR_WHEN_SUBMITTED_AND_AUDITED";
        public const string CERTIFICATES = "CERTIFICATES";
        public const string ERROR = "ERROR";
        public const string CONTACT = "CONTACT";
        public const string THE_ADMINISTRATOR = "THE_ADMINISTRATOR";
        public const string ERROR_PAGE_REASON_1 = "ERROR_PAGE_REASON_1";
        public const string ERROR_PAGE_REASON_2 = "ERROR_PAGE_REASON_2";
        public const string ERROR_PAGE_REASON_3 = "ERROR_PAGE_REASON_3";
        public const string ERROR_PAGE_REASON_4 = "ERROR_PAGE_REASON_4";
        public const string ERROR_PAGE_LINE_1 = "ERROR_PAGE_LINE_1";
        public const string IF_THE_ERROR_PERSISTS_PLEASE = "IF_THE_ERROR_PERSISTS_PLEASE";
        public const string REQUEST_ACCESS_TO_ANSWER = "REQUEST_ACCESS_TO_ANSWER";
        public const string BROWSE_AUDITED_ANSWERS = "BROWSE_AUDITED_ANSWERS";
        public const string COMPLIANCE_STATUS = "COMPLIANCE_STATUS";
        public const string DOWNLOAD_REPORT = "DOWNLOAD_REPORT";
        public const string CURRENCY_CODE_HTML = "CURRENCY_CODE_HTML";
        public const string REGISTRATION_STATUS = "REGISTRATION_STATUS";
        public const string IS_FAVOURITE = "IS_FAVOURITE";
        public const string IS_TRADING_WITH = "IS_TRADING_WITH";

        public const string PROMOTION_CODE = "PROMOTION_CODE";
        public const string DISCOUNT_PERCENTAGE = "DISCOUNT_PERCENTAGE";
        public const string PROMOTION_START_DATE = "PROMOTION_START_DATE";
        public const string PROMOTION_END_DATE = "PROMOTION_END_DATE";
        public const string SELECT_BUYER_ERROR = "SELECT_BUYER_ERROR";
        public const string PROMOTION_DATE_VALIDATION = "PROMOTION_DATE_VALIDATION";
        public const string VOUCHER_CREATION_SUCCESS = "VOUCHER_CREATION_SUCCESS";
        public const string VOUCHER_UPDATE_SUCCESS = "VOUCHER_UPDATE_SUCCESS";
        public const string PROMOTIONAL_CODE_EXISTS = "PROMOTIONAL_CODE_EXISTS";
        public const string PROMOTION_CODE_VALIDATION = "PROMOTION_CODE_VALIDATION";
        public const string PROMOTION_CODE_DISPLAY = "PROMOTION_CODE_DISPLAY";
        public const string DISCOUNT_PERCENT_DISPLAY = "DISCOUNT_PERCENT_DISPLAY";
        public const string DISCOUNT_PERCENT_RANGE = "DISCOUNT_PERCENT_RANGE";
        public const string PROMOTION_START_DATE_ERROR = "PROMOTION_START_DATE_ERROR";
        public const string PROMOTION_START_DATE_DISPLAY = "PROMOTION_START_DATE_DISPLAY";
        public const string PROMOTION_END_DATE_ERROR = "PROMOTION_END_DATE_ERROR";
        public const string PROMOTION_END_DATE_DISPLAY = "PROMOTION_END_DATE_DISPLAY";
        public const string MAP_BUYER_DISPLAY = "MAP_BUYER_DISPLAY";
        public const string UPDATE_VOUCHER = "UPDATE_VOUCHER";

    }
}
