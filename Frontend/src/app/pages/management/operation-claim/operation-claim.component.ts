import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var w2utils: any;
declare var w2popup: any;
declare var openPopup: any;
declare var openSelectedPopupGrid: any;

@Component({
  selector: 'app-operation-claim',
  templateUrl: './operation-claim.component.html',
  styleUrls: ['./operation-claim.component.css']
})
export class OperationClaimComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor() {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    if (w2ui.OperationClaimLayout) {
      w2ui.OperationClaimLayout.destroy();
    }
    $().w2layout({
      name: 'OperationClaimLayout',
      panels: [
        {
          layoutName: 'OperationClaimLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#OperationClaimLayout').w2render('OperationClaimLayout');
    if (w2ui.OperationClaimGrid) {
      w2ui.OperationClaimGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'OperationClaimGrid',
      layout: 'OperationClaimLayout',
      panel: 'main',
      show: {
        header: true,
        footer: true,
        toolbar: true,
        toolbarEdit: true,
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'OperationClaims/GetAllForUi',
        remove: this.apiUrl + 'OperationClaims/SoftDeleteForUi',
      },
      autoLoad: true,
      recid: 'Id',
      advanceOnEdit: true,
      columns: [
        {
          searchable: 'int',
          text: 'Id',
          field: 'Id',
          size: '100px',
        },
        {
          searchable: true,
          text: 'İşlem Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Açıklama',
          field: 'Description',
          size: '100px',
        },
      ],
      onAdd: function () {
        openPopup('İşlem Ekle', 'AddOperationClaimForm');
      },
      onEdit: function (event: any) {
        var selection = w2ui.OperationClaimGrid.getSelection();
        if (selection.length == 0) {
          return;
        }
        openPopup('İşlem Güncelle', 'UpdateOperationClaimForm', selection[0], {});
      }
    });
    w2ui.OperationClaimLayout.html('main', w2ui.OperationClaimGrid);
    if (w2ui.UpdateOperationClaimForm) {
      w2ui.UpdateOperationClaimForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'UpdateOperationClaimForm',
      autoSize: true,
      fields: [
        {
          type: 'int',
          text:'Id',
          field: 'Id',
          required:true,
          disabled: true,
        },
        {
          type: 'text',
          field: 'Name',
          required:true,
          disabled: true,
          html: {
            label: 'İşlem Adı',
          },
        },
        {
          type: 'textarea',
          field: 'Description',
          disabled: false,
          html: {
            label: 'Açıklama',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'OperationClaims/GetForUi',
        save: this.apiUrl + 'OperationClaims/UpdateForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.OperationClaimGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Güncelle',
          onClick(event: any) {
            w2ui.UpdateOperationClaimForm.save();
          },
        },
      },
    });

  }
}

