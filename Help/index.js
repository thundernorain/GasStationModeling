//Кнопка прокрутки к началу страницы
const scrollTop = document.getElementById('scroll-top');

document.addEventListener('scroll', () => {
    console.log(document.documentElement.clientHeight)
    //scrollTop.hidden = window.scrollY < window.innerHeight;
})

scrollTop.addEventListener('click', () => {
    window.scrollTo(0, 0);
    console.log("cdsc");
})