// loadTab - loads in the specific tab
// classId - the classId to pass in
// tab - the tab to load the content in
function loadTab(classId, tab) {
    switch(tab) {
        case "dashboard":
            return abp.ajax({
                url: 'Class/ManageClassDashboard/'
            });
    }
}