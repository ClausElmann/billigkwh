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


export interface SmartDeviceDto { 
    id?: number;
    uniqueidentifier?: string | null;
    customerId?: number | null;
    createdUtc?: string;
    latestContactUtc?: string;
    location?: string | null;
    zoneId?: number;
    maxRate?: number;
    deleted?: string | null;
    comment?: string | null;
    disableWeekends?: boolean;
    statusId?: number;
    minTemp?: number | null;
    maxRateAtMinTemp?: number | null;
    errorMail?: string | null;
}

