import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { CoursesListComponent } from './admin/Courses/courses-list.component';
import { ModuleListComponent } from './admin/Modules/module-list.component';
import { LearnerListComponent } from './admin/Learners/Learner-list.component';
import { EnrollmentListComponent } from './admin/Enrollments/enrollment-list.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent,  canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'about', component: AboutComponent, canActivate: [AppRouteGuard] },
                    { path: 'update-password', component: ChangePasswordComponent, canActivate: [AppRouteGuard] },
                    { path: 'courses', component: CoursesListComponent, data: { Permission: 'Pages.Courses' }, canActivate: [AppRouteGuard] },
                    { path: 'modules', component: ModuleListComponent, data: { Permission: 'Pages.Modules' }, canActivate: [AppRouteGuard] },
                    { path: 'learners', component: LearnerListComponent, data: { Permission: 'Pages.Learners' }, canActivate: [AppRouteGuard] },
                    { path: 'enrollments', component: EnrollmentListComponent, data: { Permission: 'Pages.Enrollments' }, canActivate: [AppRouteGuard] },

                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
