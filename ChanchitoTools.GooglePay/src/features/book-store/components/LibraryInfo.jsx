import React from "react";

export const LibraryInfo = ({ count }) => {
  if (count === 0) return null;

  return (
    <div className="bg-green-50 border border-green-200 rounded-lg p-4 mt-4">
      <h4 className="font-semibold text-green-800 mb-2">
        ✓ Your Library ({count})
      </h4>
      <p className="text-sm text-green-700">
        Books marked with "Download PDF" are yours!
      </p>
    </div>
  );
};
