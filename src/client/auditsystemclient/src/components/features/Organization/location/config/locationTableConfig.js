export const locationTableConfig = {
    locationColumns: [
        { key: 'name', label: 'Location Name' }
    ],
    
    locationActions: [
        { 
            label: 'Edit', 
            event: 'edit-item', 
            class: 'font-medium text-blue-600 dark:text-blue-500 hover:underline mr-4' 
        },
        { 
            label: 'Delete', 
            event: 'delete-item', 
            class: 'font-medium text-red-600 dark:text-red-500 hover:underline' 
        }
    ]
};
