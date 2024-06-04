import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "members", component: MembersListComponent, canActivate: [authGuard]},
  {path: "members/:id", component: MembersDetailComponent, canActivate: [authGuard]},
  {path: "lists", component: ListsComponent, canActivate: [authGuard]},
  {path: "messages", component: MessagesComponent, canActivate: [authGuard]},
  {path: "**", component: HomeComponent, pathMatch: "full"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
