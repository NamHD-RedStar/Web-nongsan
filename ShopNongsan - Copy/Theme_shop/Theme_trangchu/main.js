const headerLocation = document.querySelector('.header-location-js')
const modalPlace = document.querySelector('.modal-place')
const modalPlaceContent = document.querySelector('.modal-place_container')
const modalRegions = document.querySelectorAll('.modal-place_regions-content')
const modalRegionsIcon = document.querySelectorAll('.modal-place_regions-content>i')
const modalRegionsLists = document.querySelectorAll('.modal-place_regions-list')
const cart = document.querySelector('.header-bottom_cart.pc')
const cart2 = document.querySelector('.header-bottom_cart.reponsive')
const cartBtn = document.querySelector('.header-bottom_cart-icon1')
const cartBtn2 = document.querySelector('.header-bottom_cart-icon2')
const cartSelect = document.querySelector('.header-bottom_cart-select1')
const cartSelect2 = document.querySelector('.header-bottom_cart-select2')

const sliderImgs = document.querySelectorAll('.slider-img')
const sliderNext = document.querySelector('.slider-next')
const sliderPrev = document.querySelector('.slider-prev')
const sliderRadios = document.querySelectorAll('.slider-radio_item')
const productProposeTitles = document.querySelectorAll('.product-propose_title-name')
const productProposeNews = document.querySelectorAll('.product-propose_new')
const line = document.querySelector('.line')
const productNext = document.querySelector('.product-propose_next')
const productPrev = document.querySelector('.product-propose_prev')
const widthProductItem = document.querySelector('.product-propose_new.active>.row-nowrap>.col')
const productProposeItems = document.querySelectorAll('.product-propose_new.active>.row-nowrap>.col')
const fixedForm = document.querySelector('.fixed-form')
const btnMessage = document.querySelector('.btn-message')
const hideIcon = document.querySelector('.fixed-form_header i')
const hideBtn = document.querySelector('.fixed-btn_group-item:first-child')
const btnSrollFixed = document.querySelector('.btn-srollFixed')


let positionX = 0
let index = 0


// showModal................................................................
function showModalPlace() {
    headerLocation.addEventListener('click', () => {
        modalPlace.classList.add('open')
    })
}


//showListRegions................................................................
function showlists() {
    modalRegions[0].addEventListener('click', () => {
        modalRegionsLists[0].classList.toggle('open')
        modalRegionsIcon[0].classList.toggle('roteArrow')
        modalRegionsLists[1].classList.remove('open')
        modalRegionsIcon[1].classList.remove('roteArrow')
        modalRegionsLists[2].classList.remove('open')
        modalRegionsIcon[2].classList.remove('roteArrow')


    })
    modalRegions[1].addEventListener('click', () => {
        modalRegionsLists[1].classList.toggle('open')
        modalRegionsIcon[1].classList.toggle('roteArrow')
        modalRegionsLists[0].classList.remove('open')
        modalRegionsIcon[0].classList.remove('roteArrow')
        modalRegionsLists[2].classList.remove('open')
        modalRegionsIcon[2].classList.remove('roteArrow')

    })
    modalRegions[2].addEventListener('click', () => {
        modalRegionsLists[2].classList.toggle('open')
        modalRegionsIcon[2].classList.toggle('roteArrow')
        modalRegionsLists[1].classList.remove('open')
        modalRegionsIcon[1].classList.remove('roteArrow')
        modalRegionsLists[0].classList.remove('open')
        modalRegionsIcon[0].classList.remove('roteArrow')

    })
}
// hideModal.....................................................................
function hideModalPlace() {
    modalPlace.addEventListener('click', () => {
        modalPlace.classList.remove('open')

    })
}

function stopPropagationModal() {
    modalPlaceContent.addEventListener('click', (e) => {
        e.stopPropagation()
    })
}

// showCart..............................................................
function clickCart() {
    cart.addEventListener('click', () => {
        cartSelect.classList.toggle('openFlex')
        cartBtn.classList.toggle('onClickCart')
    })
    cart2.addEventListener('click', () => {
        cartSelect2.classList.toggle('openFlex')
        cartBtn2.classList.toggle('onClickCart')
    })
}



// next + prev + autoNext slider..........................................

function changeSlider() {

    const addAndRemoveActive = function() {
        const sliderRadioActive = document.querySelector('.slider-radio_item.active')
        sliderRadioActive.classList.remove('active')
        sliderRadios[index].classList.add('active')
    }
    const transformSliderImg = function() {
            for (const sliderImg of sliderImgs) {
                sliderImg.style.transform = `translateX(${positionX}%)`
            }
        }
        // autoChangeSlide..............................................................
    const changeNextSlide = function() {
        index++
        positionX = positionX - 100
        transformSliderImg()

        if (index >= sliderImgs.length) {
            index = 0;
            positionX = 0
            sliderImgs[0].style.transform = `translateX(${positionX}%)`
        }

        transformSliderImg()
        addAndRemoveActive()
    }
    const autoChangeSlider = setInterval(changeNextSlide, 5000)
        // nextSlide...............................

    sliderNext.addEventListener('click', () => {
            handleChangeslide(1)
        })
        // prevSlide...............................
    sliderPrev.addEventListener('click', () => {
        handleChangeslide(-1)
    })

    // handleNextAndPrev..............................................................
    function handleChangeslide(e) {
        if (e == 1) {
            changeNextSlide()

        } else if (e == -1) {
            index--
            positionX = positionX + 100
            transformSliderImg()

            if (index < 0) {
                index = sliderImgs.length - 1;
                positionX = -100 * index
                sliderImgs[0].style.transform = `translateX(${positionX}%)`
            }
            transformSliderImg()
            addAndRemoveActive()

        }

    }

    // checkedSliderRadio..............................................................
    (function checkRadioSlider() {

        sliderRadios.forEach((radio, i) => {

            radio.onclick = function() {
                index = i
                clearInterval(autoChangeSlider)
                const sliderRadioActive = document.querySelector('.slider-radio_item.active')
                sliderRadioActive.classList.remove('active')
                radio.classList.add('active')
                positionX = -i * 100
                transformSliderImg()

            }
        })
    })()

}
// changeslideProduce..............................................................

function changeSlideProduct() {
    // changeTitleAndProduce................................................................

    (function changeTitleAndProduct() {
        const productProposeTitleActive = document.querySelector('.product-propose_title-name.active')
        line.style.left = productProposeTitleActive.offsetLeft + 'px'
        line.style.width = productProposeTitleActive.offsetWidth + 'px'

        productProposeTitles.forEach((productProposeTitle, index) => {
            const productProposeNew = productProposeNews[index]

            productProposeTitle.onclick = () => {
                const productProposeTitleActive = document.querySelector('.product-propose_title-name.active')
                productProposeTitleActive.classList.remove('active')
                const productProposeNewActive = document.querySelector('.product-propose_new.active')
                productProposeNewActive.classList.remove('active')

                line.style.left = productProposeTitle.offsetLeft + 'px'
                line.style.width = productProposeTitle.offsetWidth + 'px'

                productProposeTitle.classList.add('active')
                productProposeNew.classList.add('active')

            }
        })
    })()




    const nextSlideProduct = () => {
        indexProduct++
        tranformX = tranformX - 100

        if (indexProduct > productProposeItems.length - 4) {
            indexProduct = productProposeItems.length - 4
            tranformX = -100 * indexProduct
            return
        }

        for (const productProposeItem of productProposeItems) {
            productProposeItem.style.transform = `translateX(${tranformX}%)`
        }
    }
    setInterval(nextSlideProduct, 20000)

    // nextProduct................................................................
    productNext.addEventListener('click', () => {
        handleChangeProduct(1)
    })

    //prevProduct................................................................
    productPrev.addEventListener('click', () => {
            handleChangeProduct(-1)
        })
        // handleChangeProduct................................................................
    let indexProduct = 0
    let tranformX = 0
        // autoChangeProduct................................................................
    function handleChangeProduct(e) {
        if (e == 1) {
            nextSlideProduct()

        } else if (e == -1) {
            indexProduct--
            tranformX = tranformX + 100

            if (indexProduct < 0) {
                tranformX = 0
                indexProduct = 0
                return
            }
            for (const productProposeItem of productProposeItems) {
                productProposeItem.style.transform = `translateX(${tranformX}%)`
            }

        }

    }


}



function changeMainProduct() {
    // mainProduct1..............................................................
    function changeMainProduct1() {

        const productMains = document.querySelectorAll('#product1.product-content_about.product-propose_new')
        const productMainTitles = document.querySelectorAll('#title1.product-change-title')


        productMainTitles.forEach((productMainTitle, index) => {

            const productMain = productMains[index]

            productMainTitle.onclick = () => {
                const productMainTitlesActive = document.querySelector('#title1.product-change-title.active')
                const productMainsActive = document.querySelector('#product1.product-content_about.product-propose_new.active')
                productMainTitlesActive.classList.remove('active')
                productMainsActive.classList.remove('active')

                productMainTitle.classList.add('active')
                productMain.classList.add('active')

            }
        })
    }
    changeMainProduct1()

    // mainProduct2..............................................................

    function changeMainProduct2() {


        const productMains = document.querySelectorAll('#product2.product-content_about.product-propose_new')
        const productMainTitles = document.querySelectorAll('#title2.product-change-title')

        productMainTitles.forEach((productMainTitle, index) => {

            const productMain = productMains[index]

            productMainTitle.onclick = () => {
                const productMainTitlesActive = document.querySelector('#title2.product-change-title.active')
                productMainTitlesActive.classList.remove('active')
                const productMainsActive = document.querySelector('#product2.product-content_about.product-propose_new.active')
                productMainsActive.classList.remove('active')

                productMainTitle.classList.add('active')
                productMain.classList.add('active')

            }
        })
    }
    changeMainProduct2()

    // mainProduct3..............................................................

    function changeMainProduct3() {


        const productMains = document.querySelectorAll('#product3.product-content_about.product-propose_new')
        const productMainTitles = document.querySelectorAll('#title3.product-change-title')

        productMainTitles.forEach((productMainTitle, index) => {

            const productMain = productMains[index]

            productMainTitle.onclick = () => {
                const productMainTitlesActive = document.querySelector('#title3.product-change-title.active')
                productMainTitlesActive.classList.remove('active')
                const productMainsActive = document.querySelector('#product3.product-content_about.product-propose_new.active')
                productMainsActive.classList.remove('active')

                productMainTitle.classList.add('active')
                productMain.classList.add('active')

            }
        })
    }
    changeMainProduct3()

    // mainProduct4..............................................................

    function changeMainProduct4() {


        const productMains = document.querySelectorAll('#product4.product-content_about.product-propose_new')
        const productMainTitles = document.querySelectorAll('#title4.product-change-title')

        productMainTitles.forEach((productMainTitle, index) => {

            const productMain = productMains[index]

            productMainTitle.onclick = () => {
                const productMainTitlesActive = document.querySelector('#title4.product-change-title.active')
                productMainTitlesActive.classList.remove('active')
                const productMainsActive = document.querySelector('#product4.product-content_about.product-propose_new.active')
                productMainsActive.classList.remove('active')

                productMainTitle.classList.add('active')
                productMain.classList.add('active')

            }
        })
    }
    changeMainProduct4()

    // mainProduct5..............................................................

    function changeMainProduct5() {


        const productMains = document.querySelectorAll('#product5.product-content_about.product-propose_new')
        const productMainTitles = document.querySelectorAll('#title5.product-change-title')

        productMainTitles.forEach((productMainTitle, index) => {

            const productMain = productMains[index]

            productMainTitle.onclick = () => {
                const productMainTitlesActive = document.querySelector('#title5.product-change-title.active')
                productMainTitlesActive.classList.remove('active')
                const productMainsActive = document.querySelector('#product5.product-content_about.product-propose_new.active')
                productMainsActive.classList.remove('active')

                productMainTitle.classList.add('active')
                productMain.classList.add('active')

            }
        })
    }
    changeMainProduct5()
}

// fadeAndHideForm..............................................................
function fadeAndHideForm() {
    btnMessage.addEventListener('click', () => {
        fixedForm.classList.toggle('open')
    })
    hideIcon.addEventListener('click', () => {
        fixedForm.classList.remove('open')
    })
    hideBtn.addEventListener('click', () => {
        fixedForm.classList.remove('open')
    })
}
// onScrollBtn..............................................................
function onScrollBtn() {
    window.onscroll = () => {
        if (window.scrollY >= 150) {
            btnSrollFixed.style.transform = 'translateY(0)'

        } else {
            btnSrollFixed.style.transform = `translateY(100px)`

        }
    }
}

// menuResponsive..............................................................
function menuResponsive() {
    const menuBtn = document.querySelector('.header-bottom_menu-icon')
    const exitMenu = document.querySelector('.nav-list_responsive-icon')
    const overlayMenu = document.querySelector('.header-bottom_menu-list')
    const menuResponsive = document.querySelector('.nav-list_responsive')

    menuBtn.addEventListener('click', () => {
        overlayMenu.style.transform = `translateX(0)`

    })
    exitMenu.addEventListener('click', () => {
        overlayMenu.style.transform = `translateX(-100%)`

    })
    overlayMenu.addEventListener('click', () => {
        overlayMenu.style.transform = `translateX(-100%)`
    })

    function stopPropagationModal() {
        menuResponsive.addEventListener('click', (e) => {
            e.stopPropagation()
        })
    }
    stopPropagationModal()
}


// callFunction..............................................................
showModalPlace()
showlists()
hideModalPlace()
stopPropagationModal()
clickCart()
changeSlider()
changeSlideProduct()
changeMainProduct()
fadeAndHideForm()
onScrollBtn()
menuResponsive()