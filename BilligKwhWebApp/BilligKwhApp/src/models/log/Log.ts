export class LogEvent {
  // Props
  public id: number;
  public logLevelName: string;
  public shortMessage: string;
  public fullMessage: string;
  public ipAddress: string;
  public pageUrl: string;
  public referrerUrl: string;
  public dateCreatedUtc: string;
  public module: string;
  public dataObject: string;
  public userId?: number;
}
