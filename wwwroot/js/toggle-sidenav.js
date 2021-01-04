window.addEventListener("load", function(event){
    var sidebar=   document.getElementById("sidebar");
    var sidebarToggler = document.getElementById("sidebar-toggler")
    sidebarToggler.addEventListener("click", function(event){
        event.preventDefault();
        // sidebar.style.width = sidebar.style.width === "100%"? "0%": "100%";
       if(sidebar.classList.contains("anim-sidebar"))
       {
           sidebar.classList.remove("anim-sidebar");
       }else{
           sidebar.classList.add("anim-sidebar");
       }
    })
});