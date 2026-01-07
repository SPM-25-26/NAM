/**
 * Represents the configuration for a single step in a stepper component.
 *
 * @template TState - The type of the state object passed to the render function.
 * @property label - The display label for the step.
 * @property [description] - An optional description providing additional details about the step.
 * @property render - A function that receives the current state and returns the React node to render for this step.
 */
export type StepConfig<TState> = {
  label: string;
  description?: string;
  render: (state: TState) => React.ReactNode;
};
