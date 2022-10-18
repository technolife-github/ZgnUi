import { VehicleComponent } from './vehicle/vehicle.component';
import { AddTransactionComponent } from './add-transaction/add-transaction.component';
import { PendingTransactionComponent } from './pending-transaction/pending-transaction.component';
import { TransactionComponent } from './transaction/transaction.component';
import { StationComponent } from './station/station.component';
import { OperationClaimComponent } from './operation-claim/operation-claim.component';
import { UserComponent } from './user/user.component';
import { AuthorityComponent } from './authority/authority.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'authorities', component: AuthorityComponent},
  {path:'operation-claims', component: OperationClaimComponent},
  {path:'users', component: UserComponent},
  {path:'stations', component: StationComponent},
  {path:'vehicles', component: VehicleComponent},
  {path:'transactions', component: TransactionComponent},
  {path:'pending-transactions', component: PendingTransactionComponent},
  {path:'add-transaction', component: AddTransactionComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
