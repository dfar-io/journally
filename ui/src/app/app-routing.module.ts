import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EntriesPageComponent } from './entries-page/entries-page.component';
import { EntryPageComponent } from './entry-page/entry-page.component';
import { EntryResolver } from './entry-page/entry-page.resolver';
import { AuthGuard } from './shared/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: EntryPageComponent
  },
  {
    path: 'entries/:id',
    component: EntryPageComponent,
    canActivate: [AuthGuard],
    resolve: { resolvedEntry: EntryResolver }
  },
  {
    path: 'entries',
    component: EntriesPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '*',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
