export const getRateColorClasses =  (level) => {
    
    if (!level) {
        return 'px-2 py-1 rounded-md font-semibold text-xs bg-gray-100 text-gray-800 border border-gray-200';
    }
    
    const levelStr = String(level).toLowerCase();
    const colorMap = {
        critical: 'px-2 py-1 rounded-md font-semibold text-xs bg-red-100 text-red-800 border border-red-200',
        high: 'px-2 py-1 rounded-md font-semibold text-xs bg-orange-100 text-orange-800 border border-orange-200',
        medium: 'px-2 py-1 rounded-md font-semibold text-xs bg-yellow-100 text-yellow-800 border border-yellow-200',
        low: 'px-2 py-1 rounded-md font-semibold text-xs bg-blue-100 text-blue-800 border border-blue-200',
        minimal: 'px-2 py-1 rounded-md font-semibold text-xs bg-green-100 text-green-800 border border-green-200'
      };

    return colorMap[levelStr] || 'px-2 py-1 rounded-md font-semibold text-xs bg-gray-100 text-gray-800 border border-gray-200'
};