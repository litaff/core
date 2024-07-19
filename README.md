# Core
My core C# libraries. Contains:
- `StateMachine`: A simple state machine implementation. 
Allows you to define states and transitions between them. Each state can have an entry and exit action.
- `Logger`: A simple logging interface, 
with a default implementation that logs to the console with simple formatting.
- `ViewManager`: A library which introduces the concept of views and panels. ViewManager aggregates all views and
allows you to fetch them by type, while making sure only one view is visible at a time.
PanelManager aggregates all panels and allows you to fetch them by type or id.
The core concept is that a view is a collection of panels. Panels are the individual components that make up a view
or exist on their own. Panels outside a view should be reusable and be contained by PanelManager, 
because they are to be used at any point. Panels inside a view are specific to that view and should not be 
accessible from PanelManager. Views should only be accessed through ViewManager.
- `Pooling`: A simple pooling system, which keeps track of active poolables and allows the user to get an available
poolable, if they exist. If there are no avaiable poolables, a new one is created. The system allows the user to free
all active poolables or free them individually, if they have a reference to it. The implementation of the poolable 
interface should invoke the provided event, as the system depends on the Free method and the event to work.