import { Observable } from "rxjs";

export interface BiWizardStep {
  /**
   * To handle when the current step in the wizard is about to be changed. What to do BEFORE or AFTER step changes
   */
  handleOnWizardStepChange: (isForward: boolean) => Observable<WizardStepChangeResult>;
}

/**
 * Type of the observable result returned by handleOnWizardStepChange
 */
export interface WizardStepChangeResult {
  /**
   * Should we actually change the step after this handling? Sometimes this is not the case (either
   * because of error or because maybe clicking the next button triggers some operation before allowing to proceed )
   */
  changeStep: boolean;

  /**
   * If any error, this is where to put the error message for display
   */
  errorMessage?: string;
}
