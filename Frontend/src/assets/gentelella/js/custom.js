const getClassList =(classStr)=>{
  return classStr?.split(/\s+/) ?? [];
};
const removeClicked=()=>{
  $(".side-menu li").removeClass("active");
  $(".side-menu li").removeClass("current-page");
  $(".side-menu li").removeClass("sub_menu");
  $(".side-menu ul").css("display","none");
};
const sidebarFunc = (link)=>{
  var $a = $(link);
  var $li = $a.parent();
  var $ul = $li.parent();
  var classes=getClassList($li.attr("class"));
  if(classes.includes("parent_link")){
    if(classes.includes("active")){
      setTimeout(() => {
        $li.removeClass("active");
        if ($li.find("ul").length) {
          $li.children("ul").css("display", "none");
        }
      }, 100);
       return;
    }
    removeClicked();
    setTimeout(() => {
      $li.addClass("active");
      $li.children("ul").css("display", "block");
    }, 100);
  }else if(classes.includes("child_link")){
    $parent_li=$ul.parent();
    $parent_ul=$parent_li.children("ul");
    if(classes.includes("current-page")){
      setTimeout(() => {
        $li.removeClass("current-page");
        if ($li.find("ul").length) {
          $li.children("ul").css("display", "none");
        }
      }, 100);
       return;
    }
    removeClicked();
    setTimeout(() => {
      $li.addClass("current-page");
      $li.children("ul").css("display", "block");
      $parent_li.addClass("active");
      $parent_ul.css("display", "block");
    }, 100);
  }else if(classes.includes("inchild_link")){
    $sub_li=$ul.parent();
    $sub_ul=$sub_li.children("ul");
    $parent_ul=$sub_li.parent();
    $parent_li=$parent_ul.parent();
    if(classes.includes("current-page")){
       return;
    }
    removeClicked();
    setTimeout(() => {
      $li.addClass("sub_menu current-page");
      $li.children("ul").css("display", "block");
      $parent_li.addClass("active");
      $parent_ul.css("display", "block");
      $sub_li.addClass("active");
      $sub_ul.css("display", "block");
    }, 100);
  }
}
var isInit = false;
const initSidebar = (btn) => {
  if(isInit) return;
  isInit=true;
  $(".side-menu a").on('click', function (ev) {
    sidebarFunc(this);
  });
  var CURRENT_URL = window.location.href.split('#')[0].split('?')[0];
  var link=$(".side-menu a").filter(function () {
      return this.href == CURRENT_URL;
  })
  sidebarFunc(link[0]);
}

function clearSpecialCharacter(str) {
  var charMap = {
    Ç: 'c',
    Ö: 'o',
    Ş: 's',
    İ: 'i',
    I: 'i',
    Ü: 'u',
    Ğ: 'g',
    ç: 'c',
    ö: 'o',
    ş: 's',
    ı: 'i',
    ü: 'u',
    ğ: 'g'
  };
  str_array = str.split('');
  for (var i = 0, len = str_array.length; i < len; i++) {
    str_array[i] = charMap[str_array[i]] || str_array[i];
  }
  str = str_array.join('');
  return str.replace(" ", "_").replace("__", "_").replace(/[^a-z0-9-.çöşüğı]/gi, "").toLowerCase();
}

const BACKUP = [];
function openPopup(
  title = "",
  content = "",
  recid = null,
  record = {},
  openMaximized = false
) {
  if ($("#form").length > 0) $("#form").remove();
  w2popup.open({
    title: title,
    body: '<div id="form" style="width: 100%; height: 100%;"></div>',
    style: "padding: 15px 0px 0px 0px",
    width: w2ui[content].width == undefined ? 600 : w2ui[content].width,
    height: w2ui[content].height == undefined ? 400 : w2ui[content].height,
    showMax: true,
    innerContent: [content],
    showBack: BACKUP.filter(function () { return BACKUP[content] == undefined }).length > 1,
    openMaximized: openMaximized,
    onToggle: function (event) {
      $(w2ui[content].box).hide();
      event.onComplete = function () {
        $(w2ui[content].box).show();
        w2ui[content].resize();
        Object.keys(w2ui).forEach(key => {
          if (w2ui[key].constructor.name == "w2form") {
            w2ui[key].resize();
            if (w2ui[key].tabs.click)
              w2ui[key].tabs.click(w2ui[key].tabs.active);
          }
        })
      };
    },
    onOpen: function (event) {
      event.onComplete = function () {
        setTimeout(() => {
          $("#form").html("");
          if (w2ui[content] != undefined) {
            console.log(record);
            w2ui[content].record = record;
            w2ui[content].refresh();
          }
          w2ui[content].recid = recid;
          if (recid != null) {
            w2ui[content].reload();
          }
          $("#w2ui-popup #form").w2render(content);
          let view = {};
          view[content] = "#form";
          if (BACKUP.filter(function () { return BACKUP[content] == undefined }).length == 0)
            BACKUP.push(view);
          w2ui[content].resize();
          w2ui[content].refresh();
        }, 300);
      };
    },
    onClose: function (event) {
      event.onComplete = function () {
        BACKUP.pop();
      }
    }
  });
}
function openPopupGrid(
  title = "",
  content = "",
  postData = {},
  openMaximized = true
) {
  w2popup
    .open({
      title: title,
      width: 700,
      height: 500,
      innerContent: [content],
      showMax: openMaximized,
      showBack: BACKUP.filter(function () { return BACKUP[content] == undefined }).length > 1,
      body: '<div id="popup-grid" style="position: absolute; left: 2px; right: 2px; top: 0px; bottom: 3px;"></div>',
      onToggle(event) {
        event.done(() => {
          w2ui[content].resize();
          w2ui[content].refresh();
        });
      }
    })
    .then((e) => {
      var obj = this;
      const p = new URLSearchParams(postData).toString();
      w2ui[content].url.get = w2ui[content].url.get.split('?')[0] + "?" + p;
      $("#w2ui-popup #popup-grid").w2render(content);
      setTimeout(() => {
        BACKUP.push(content);
        w2ui[content].resize();
      }, 300);
    });
}

function openSelectedPopupGrid(
  title = "",
  contentLeft = "",
  contentRight = "",
  postData = {},
  openMaximized = true
) {
  w2popup
    .open({
      title: title,
      width: 700,
      height: 500,
      showMax: openMaximized,
      body: `<div style="position: relative; height: calc(100% - 10px );">
          <div id="grid1" style="position: absolute; left: 0px; width: 49.9%; height: 100%;"></div>
          <div id="grid2" style="position: absolute; right: 0px; width: 49.9%; height: 100%;"></div>
      </div>`,
      onToggle(event) {
        event.done(() => {
          w2ui[contentLeft].resize();
          w2ui[contentRight].resize();
        });
      },
    })
    .then((e) => {
      const p = new URLSearchParams(postData).toString();
      w2ui[contentLeft].url.get = w2ui[contentLeft].url.get.split('?')[0] + "?" + p;
      w2ui[contentRight].url.get = w2ui[contentRight].url.get.split('?')[0] + "?" + p;
      w2ui[contentRight].url.save = w2ui[contentRight].url.save.split('?')[0] + "?" + p;
      $("#w2ui-popup #grid1").w2render(contentLeft);
      $("#w2ui-popup #grid2").w2render(contentRight);
      console.log(w2ui[contentLeft]);
      console.log(w2ui[contentRight]);
    });
}

let ExcelToJSON = async function (file, items = [], callback) {
  var reader = new FileReader();
  reader.onload = function (e) {
    var data = e.target.result;
    var workbook = XLSX.read(data, {
      type: 'binary'
    });
    var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[workbook.SheetNames[
      0]]);
    var keys = Object.keys(XL_row_object[0]);
    console.log(XL_row_object);
    var jsonStringData = JSON.stringify(XL_row_object);
    var headersData = [{
      field: "recid",
      text: "Id",
      size: '60px'
    }, {
      field: "excelDesc",
      text: w2utils.lang("excelDesc"),
      size: '100px'
    }];
    var formField = [{
      field: 'overwrite',
      type: 'toggle',
      html: {
        label: w2utils.lang('overwrite'),
      }
    }];
    var searches = [];
    keys.forEach((element, index) => {
      jsonStringData = jsonStringData.replaceAll('"' + element + '"', '"' +
        clearSpecialCharacter(element) + '"');
      keys[index] = clearSpecialCharacter(element);
      headersData[headersData.length] = {
        field: keys[index],
        text: element,
        size: '120px'
      };
      formField[formField.length] = {
        field: keys[index],
        type: 'list',
        required: false,
        html: {
          label: element,
          attr: 'style="width: 100%;"'
        },
        options: {
          items: items
        }
      };
      searches[searches.length] = {
        field: keys[index],
        label: element,
        type: 'text'
      };
    });
    var jsonData = JSON.parse(jsonStringData);
    jsonData.forEach((element, index) => {
      jsonData[index].recid = index + 1;
    });
    callback({
      status: true,
      headers: headersData,
      fields: formField,
      json: jsonData,
      searches: searches
    });

  };

  reader.onerror = function (ex) {
    callback({
      status: true,
      error: ex
    });
  };
  setTimeout(() => {
    reader.readAsBinaryString(file);
  }, 300);
};

function SelectExcelImport(importFields = [], url = "") {
  if (w2ui.ExcelImport) {
    w2ui.ExcelImport.destroy();
  }
  $().w2form({
    name: "ExcelImport",
    fields: [
      { "field": "ExcelFile", "html": { "label": w2utils.lang("ExcelFile") }, "type": "file", "options": { "max": "1" }, "required": 0, "hidden": 0, "disabled": 0 }
    ],
    "actions": {
      "Cancel": function () { w2popup.close(); },
      "Save": function () { this.save(); }

    },
    "record": { "ExcelFile": "" },
    "onAction": function (event) {
      var obj = this;
      obj.lock(w2utils.lang('ExcelReading'), true);
      var files = this.record.ExcelFile;
      if (files.length > 0 && event.target == 'Save') {
        ExcelToJSON(files[0].file, importFields,
          function (result) {
            obj.unlock();
            if (!result.status) {
              w2alert(result.error);
              return;
            }
            ImportExcelSetting(url, result);
          });
      }
    }
  });
  w2popup.open({
    title: 'Excel',
    body: '<div id="excelImportForm" style="width: 100%; height: 100%;"></div>',
    style: 'padding: 15px 0px 0px 0px',
    width: 500,
    height: 350,
    showMax: true,
    onToggle: function (event) {
      $(w2ui.ExcelImport.box).hide();
      event.onComplete = function () {
        $(w2ui.ExcelImport.box).show();
        w2ui.ExcelImport.resize();
      }
    },
    onOpen: function (event) {
      event.onComplete = function () {
        $('#w2ui-popup #excelImportForm').w2render('ExcelImport');

        setTimeout(function () {
          w2ui.ExcelImport.refresh();
        }, 300);
      }
    }
  });
}

function ImportExcelSetting(url = "", data) {
  let config = {
    grid: {
      show: {
        toolbar: true,
        footer: true,
        toolbarDelete: true,
        toolbarSave: true
      },
      searches: data.searches,
      columns: data.headers,
      records: data.json,
      name: 'ImportGrid',
      style: 'border: 1px solid #efefef',
    },
    form: {
      url: url,
      header: w2utils.lang('SelectField'),
      name: 'ImportForm',
      style: 'border: 1px solid #efefef',
      fields: data.fields,
      actions: {
        Save() {
          let errors = this.validate()
          if (errors.length > 0) return
          console.log(this);
          this.record["data"] = JSON.stringify(w2ui.ImportGrid.records);
          this.save();
        }
      }
    }
  };
  if (w2ui.ImportGrid) w2ui.ImportGrid.destroy();
  if (w2ui.ImportForm) w2ui.ImportForm.destroy();
  let grid = new w2grid(config.grid)
  let form = new w2form(config.form)
  w2popup.open({
    title: w2utils.lang('ImportFromExcel'),
    width: 900,
    height: 600,
    showMax: true,
    body: `<div style="position: relative; height: calc(100% - 10px );">
      <div id="grid1" style="position: absolute; left: 0px; width: 64.9%; height: 100%;"></div>
      <div id="grid2" style="position: absolute; right: 0px; width: 34.9%; height: 100%;"></div>
  </div>`,
    onToggle(event) {
      event.done(() => {
        w2ui["ImportGrid"].resize();
        w2ui["ImportForm"].resize();
      });
    },
    onOpen(event) {
      event.done(() => {
        setTimeout(() => {
          w2ui["ImportGrid"].resize();
          w2ui["ImportForm"].resize();
          w2ui["ImportGrid"].refresh();
          w2ui["ImportForm"].refresh();
        }, 300);
      });
    },
  })
    .then(e => {
      $("#w2ui-popup #grid1").w2render("ImportGrid");
      $("#w2ui-popup #grid2").w2render("ImportForm");
    })

}

function getOptionList(form, changeField, field, searchValue, url) {
  var request = {
    "limit": -1,
    "offset": 0,
    "searchLogic": "AND",
    "search": [
      { "field": field, "type": "list", "operator": "is", "value": searchValue.id, "text": searchValue.text }
    ]
  }
  let ajaxOptions = {
    type: "GET",
    url: url,
    data: { request: JSON.stringify(request) },
    dataType: "JSON",
    headers: {}
  };
  if (localStorage.getItem("token") != null)
    ajaxOptions.headers["Authorization"] =
      "Bearer " + localStorage.getItem("token");
  $.ajax(ajaxOptions)
    .done((data, status, xhr) => {
      if (xhr.status != 200) {
        w2alert(xhr.responseText);
        return;
      }
      form.fields.forEach(f => {
        if (f.name == changeField) {
          f.options.items = data.records.map(e => { return { id: e.Id, text: e.Name } });
          form.record[changeField] = null;
          form.refresh();
        }
      });
    });
}

function initializeOptionData(form, changeField, url,idField="Id",textField="Name") {
  let ajaxOptions = {
    type: "GET",
    url: url,
    dataType: "JSON",
    headers: {}
  };
  if (localStorage.getItem("token") != null)
    ajaxOptions.headers["Authorization"] =
      "Bearer " + localStorage.getItem("token");
  $.ajax(ajaxOptions)
    .done((data, status, xhr) => {
      if (xhr.status != 200) {
        w2alert(xhr.responseText);
        return;
      }
      w2ui[form].fields.forEach(f => {
        if (f.field == changeField) {
          let items=[];
          data.Data.forEach(e => {
            var getText=(tf,el)=>{
              if(tf.indexOf("{")>-1){
                var re=new RegExp("{(.*?)}","g");
                var matches = [...tf.matchAll(re)];
                matches.forEach(x => {
                  tf=tf.replace(x[0],el[x[1]]);
                });
                return tf;
              }
              else{
                return el[tf];
              }
            };
            items.push({ id: e[idField], text:getText(textField,e), obj:e })  });
          f.options.items = items;
          w2ui[form].record[changeField] = null;
          w2ui[form].refresh();
          w2ui[form].unlock();
        }
      });
    });
}
function urlParamsToObject(url) {
  var params = url.split('?')[1];
  return JSON.parse('{"' + decodeURI(params).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}')
}
