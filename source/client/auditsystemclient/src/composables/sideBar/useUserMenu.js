import { ref } from 'vue';

export function useUserMenu() {
    const userMenuRef = ref(null);
    
    const toggleUserMenu = (event) => {
        if (event) {
            event.stopPropagation();
        }
        if (userMenuRef.value && userMenuRef.value.toggleDropdown) {
            userMenuRef.value.toggleDropdown();
        } else {
            console.error('SideBar.vue: userMenuRef or toggleDropdown not available. userMenuRef.value', userMenuRef.value);
            if (userMenuRef.value) {
                console.error('SideBar.vue: userMenuRef.value properties:', Object.keys(userMenuRef.value))
            }
        }
    }

    return {
        userMenuRef,
        toggleUserMenu
    }
}