//generate images for diffrent screen size
desktopImageMeguaMenu = {
    active: '/Content/Images/Hadi/arrow_up-03.svg',
    deactive: "/Content/Images/Hadi/arrow_down-03.svg"
};
mobileImageMeguaMenu = {
    active: '/Content/Images/Hadi/dismiss.svg',
    deactive: "/Content/Images/Hadi/1x/burger.png"
};


//change color and image of link which come from param 
function toggleLink(elm, desScreen) {

    elm.classList.toggle("active");
    let childrenList = elm.children;
    let imageElm = childrenList[0];
    var hasActiveClass = elm.classList.contains("active");

    if (desScreen) {
        changeImage(hasActiveClass, desktopImageMeguaMenu, imageElm)
    } else {
        changeImage(hasActiveClass, mobileImageMeguaMenu, imageElm)
    }

    displayMegaMenu();
}

//change image by exist 'active' class or not
function changeImage(existClass, imageList, image) {

    if (existClass) {
        image.src = imageList.active;
    } else {
        image.src = imageList.deactive;
    }
}

//show and hide megaMenu of navbar
function displayMegaMenu() {

    let megaMenu = document.querySelector(".mega-menu");
    megaMenu.classList.toggle("show");
}

//go to top when click
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

function goToAnArticle(id) {
    window.location = '/Articles/Details/' + id;
}

function goToContactUs() {
    window.location='/Home/Contact';
}

function goToPage(link ,element) {
    window.location = link;
    element.classList.toggle('active');
}


//show mobile menu
function toggleLink2(elm, desScreen) {

    elm.classList.toggle("active");
    let childrenList = elm.children;
    let imageElm = childrenList[0];
    var hasActiveClass = elm.classList.contains("active");

    if (desScreen) {
        changeImage(hasActiveClass, desktopImageMeguaMenu, imageElm)
    } else {
        changeImage(hasActiveClass, mobileImageMeguaMenu, imageElm)
    }

    displayMegaMenu2();
}
//show and hide megaMenu of navbar
function displayMegaMenu2() {

    let megaMenu = document.querySelector(".mobile-menu");
    megaMenu.classList.toggle("show");
}

function changeMegaMenuImage(elm, desScreen) {

    elm.classList.toggle("active");
    let childrenList = elm.children;
    let imageElm = childrenList[0];
    var hasActiveClass = elm.classList.contains("active");

    if (desScreen) {
        changeImage(hasActiveClass, desktopImageMeguaMenu, imageElm)
    } else {
        changeImage(hasActiveClass, mobileImageMeguaMenu, imageElm)
    }

    
}

