import { Observable, BehaviorSubject } from "rxjs";
/**
 * Interface for a custom store that store's a part of the application's state data.
 * Where storage of state data is relevant in the application, extend this interface with a class named after
 * the feature area. Like "PoslistStore" or something.
 * Inpired by this great article: https://jurebajt.com/state-management-in-angular-with-observable-store-services/.
 **/
export class BiStore<T> {
  protected state: BehaviorSubject<T>;

  /**
   * The current, observable state value
   */
  public state$: Observable<T>;

  protected constructor(initialState: T) {
    this.state = new BehaviorSubject(initialState);
    this.state$ = this.state.asObservable();
  }

  /**
   * Returns the current, raw value of the state
   */
  public getCurrentStateValue(): T {
    return this.state.value;
  }

  public setState(state: T) {
    this.state.next(state);
  }
}
