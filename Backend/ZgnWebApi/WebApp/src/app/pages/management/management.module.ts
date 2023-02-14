import { SelectStationComponent } from './components/select-station/select-station.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementComponent } from './management.component';
import { ManagementRoutingModule } from './management-routing.module';
import { AuthorityComponent } from './authority/authority.component';
import { UserComponent } from './user/user.component';
import { OperationClaimComponent } from './operation-claim/operation-claim.component';
import { StationComponent } from './station/station.component';
import { TransactionComponent } from './transaction/transaction.component';
import { PendingTransactionComponent } from './pending-transaction/pending-transaction.component';
import { AddTransactionComponent } from './add-transaction/add-transaction.component';
import { VehicleComponent } from './vehicle/vehicle.component';
import { AddTransactionv2Component } from './add-transactionv2/add-transactionv2.component';
import {  FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StationDesignerComponent } from './station-designer/station-designer.component';
import { StationDesignViewerComponent } from './components/station-design-viewer/station-design-viewer.component';
import { CommitTransactionComponent } from './commit-transaction/commit-transaction.component';

@NgModule({
  declarations: [
    ManagementComponent,
    HomeComponent,
    AuthorityComponent,
    UserComponent,
    OperationClaimComponent,
    StationComponent,
    TransactionComponent,
    PendingTransactionComponent,
    AddTransactionComponent,
    VehicleComponent,
    AddTransactionv2Component,
    SelectStationComponent,
    StationDesignerComponent,
    StationDesignViewerComponent,
    CommitTransactionComponent
  ],
  imports: [
    CommonModule,
    ManagementRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    ManagementComponent
  ]
})
export class ManagementModule { }
