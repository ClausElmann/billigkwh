/**
 * BilligKwh API Documentation
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


export interface CustomerModel { 
    id?: number;
    name?: string | null;
    address?: string | null;
    zipcode?: number;
    city?: string | null;
    displayAddress?: string | null;
    deleted?: boolean;
    languageId?: number;
    countryId?: number;
    companyRegistrationId?: number | null;
    timeZoneId?: string | null;
    hourWage?: number;
    coveragePercentage?: number;
    economicId?: number | null;
    invoiceMail?: string | null;
    invoiceContactPerson?: string | null;
    invoicePhoneFax?: string | null;
    invoiceMobile?: string | null;
}

