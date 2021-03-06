import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { UserEditComponent } from './../components/users/user-edit/user-edit.component';

@Injectable({ providedIn: 'root' })
export class PreventUnsavedChanges implements CanDeactivate<UserEditComponent> {
  constructor() {}

  canDeactivate(component: UserEditComponent) {
    if (component.editForm.dirty) {
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost.');
    }
    return true;
  }
}
